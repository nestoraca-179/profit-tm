using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using ProfitTM.Models;
using Quartz;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quartz.Impl;

namespace ProfitTM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const int QUARTZ_INTERVAL_MINUTES = 3;
        private const int QUARTZ_BATCH_SIZE = 200;
        private static readonly object schedulerLock = new object();
        private static IScheduler scheduler;

        protected void Application_Init()
        {
            // Incident.CreateIncident("APPLICATION INIT", new Exception());
        }

        protected void Application_Start()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
            DevExpress.Web.Mvc.MVCxWebDocumentViewer.StaticInitialize();
            Incident.CreateIncident("APPLICATION START", new Exception());

            #region QUARTZ
            lock (schedulerLock)
            {
                if (scheduler == null)
                {
                    StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
                    scheduler = schedulerFactory.GetScheduler().Result;

                    var job = JobBuilder.Create<EnvioDocumentos>()
                        .WithIdentity("myJob", "group1")
                        .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("myTrigger", "group1")
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInMinutes(QUARTZ_INTERVAL_MINUTES)
                            // .WithIntervalInSeconds(30) // PRUEBAS
                            .WithMisfireHandlingInstructionNowWithExistingCount()
                            .RepeatForever())
                        .Build();

                    scheduler.ScheduleJob(job, trigger);
                }

                if (!scheduler.IsStarted)
                    scheduler.Start();
            }
            #endregion
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = HttpContext.Current.Server.GetLastError();
            Incident.CreateIncident("APPLICATION ERROR", ex);
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        protected void Application_Disposed()
        {
            // Incident.CreateIncident("APPLICATION DISPOSED", new Exception());
        }

        protected void Application_End()
        {
            try
            {
                lock (schedulerLock)
                {
                    if (scheduler != null)
                    {
                        scheduler.Shutdown(false).GetAwaiter().GetResult();
                        scheduler = null;
                    }
                }

                // HttpContext.Current.Session.Clear();
                // HttpContext.Current.Session.Abandon();
                // HttpContext.Current.Session.RemoveAll();

                // Incident.CreateIncident("APPLICATION END", new Exception());
            }
            catch (Exception ex)
            {
                Incident.CreateIncident("ERROR EN APPLICATION END", ex);
            }
        }

        protected void Session_Start()
        {
            // Incident.CreateIncident("SESSION START", new Exception());
        }

        protected void Session_End()
        {
            // Incident.CreateIncident("SESSION END", new Exception());
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        #region QUARTZ
        [DisallowConcurrentExecution]
        public class EnvioDocumentos : IJob
        {
            async Task IJob.Execute(IJobExecutionContext context)
            {
                List<int> conn_error = new List<int>();
                List<LogsFactOnline> logs = LogsFact.ClaimPendingLogs(QUARTZ_BATCH_SIZE);

                if (logs.Count == 0)
                    return;

                foreach (LogsFactOnline log in logs)
                {
                    if (conn_error.Contains(log.ConnID))
                        continue;

                    try
                    {
                        LogsFact.CreateProcessingTrace(log, "CLAIMED", "Inicio de procesamiento del log");

                        if (LogsFact.RequiresControlSync(log))
                        {
                            try
                            {
                                LogsFact.CreateProcessingTrace(log, "CONTROL_SYNC", $"Reintentando actualizacion local con n_control {log.NroControl}");
                                Invoice.UpdateControl(log, log.NroControl);
                                log.Status = (int)LogStatus.SENTSTATUS;
                                log.Message = "CONTROL UPDATED";
                                log.HttpCode = string.IsNullOrEmpty(log.HttpCode) ? "200" : log.HttpCode;
                                LogsFact.CreateProcessingTrace(log, "CONTROL_SYNC", "Actualizacion local completada sin reenvio a ATF");
                            }
                            catch (Exception ex)
                            {
                                log.Status = (int)LogStatus.CONTROLPENDINGSTATUS;
                                log.Message = $"CONTROL PENDING: {ex.Message}";
                                LogsFact.CreateProcessingTrace(log, "CONTROL_SYNC_RETRY_FAILED", ex.Message);
                            }

                            continue;
                        }

                        Connections conn = Connection.GetConnByID(log.ConnID.ToString());
                        conn = await Connection.EnsureValidTokenAsync(conn);

                        ModelInvoiceInfoResponse info = await new Root().SendInvoiceInfoAsync(log, conn.Token);

                        if (info.codigo == "200" || info.codigo == "201")
                        {
                            log.DateSent = DateTime.Now;
                            log.NroControl = info.resultado?.numeroControl;
                            log.Message = "ATF ACCEPTED";
                            log.HttpCode = "200";

                            if (string.IsNullOrWhiteSpace(log.NroControl))
                                throw new Exception($"ATF acepto el documento {log.NroFact} pero no retorno numero de control.");

                            LogsFact.CreateProcessingTrace(log, "ATF_ACCEPTED", $"Numero de control recibido {log.NroControl}");

                            try
                            {
                                Invoice.UpdateControl(log, log.NroControl);
                                log.Status = (int)LogStatus.SENTSTATUS;
                                log.Message = "OK";
                                LogsFact.CreateProcessingTrace(log, "CONTROL_UPDATED", $"Control {log.NroControl} aplicado localmente");
                            }
                            catch (Exception ex)
                            {
                                log.Status = (int)LogStatus.CONTROLPENDINGSTATUS;
                                log.Message = $"CONTROL PENDING: {ex.Message}";
                                LogsFact.CreateProcessingTrace(log, "CONTROL_PENDING", ex.Message);
                            }
                        }
                        else if (info.codigo == "203" || info.codigo == "400")
                        {
                            if (log.Status != (int)LogStatus.WAITINGSTATUS)
                            {
                                ModelAssignRequest assign = new ModelAssignRequest()
                                {
                                    detalleAsignacion = new List<DetalleAsignacion>()
                                    {
                                        new DetalleAsignacion()
                                        {
                                            serie = log.Serie.Trim(),
                                            tipoDocumento = log.NroFact.Contains("N-") ? "02" : "01",
                                            numeroDocumentoInicio = log.NroFact.Replace("N-", ""),
                                            numeroDocumentoFin = log.NroFact.Replace("N-", "")
                                        }
                                    }
                                };
                                ModelAssignResponse response = await new Root().SendAssign(assign, conn.Token);

                                log.Status = (int)LogStatus.WAITINGSTATUS; // WAITING
                                log.Message = "WAITING FOR RE-SEND";
                                log.HttpCode = info.codigo;
                                LogsFact.CreateProcessingTrace(log, "ASSIGN_WAIT", $"ATF respondio {info.codigo}. Numeracion solicitada.");
                            }
                            else
                            {
                                if (info.validaciones != null)
                                {
                                    string msg = "";
                                    foreach (string v in info.validaciones)
                                        msg += (v + "-");

                                    Incident.CreateIncident("ERROR DE REENVIO DE INFORMACION DE FACTURA " + log.NroFact, new Exception(msg));
                                }
                            }
                        }
                    }
                    catch (AuthenticationException ex)
                    {
                        log.Status = (int)LogStatus.ERRORSTATUS; // ERROR AUTHENTICATION
                        log.Message = ex.Message;
                        log.HttpCode = ex.Message.Split(new string[] { " ** " }, StringSplitOptions.RemoveEmptyEntries)[1];

                        Incident.CreateIncident($"ERROR EN AUTENTICACION DE ATF {log.NroFact}", ex);
                    }
                    catch (InformationException ex)
                    {
                        log.Status = (int)LogStatus.ERRORSTATUS; // ERROR INFORMATION
                        log.Message = ex.Message;
                        log.HttpCode = ex.Message.Split(new string[] { " ** " }, StringSplitOptions.RemoveEmptyEntries)[1];

                        Incident.CreateIncident($"ERROR EN INFORMACION DE ATF {log.NroFact}", ex);
                    }
                    catch (AssignmentException ex)
                    {
                        log.Status = (int)LogStatus.ERRORSTATUS; // ERROR ASSIGNMENT
                        log.Message = ex.Message;
                        log.HttpCode = ex.Message.Split(new string[] { " ** " }, StringSplitOptions.RemoveEmptyEntries)[1];

                        Incident.CreateIncident($"ERROR EN ASIGNACION DE ATF {log.NroFact}", ex);
                    }
                    catch (Exception ex)
                    {
                        log.Status = (int)LogStatus.ERRORSTATUS; // ERROR GENERAL
                        log.Message = ex.Message;

                        Incident.CreateIncident("ERROR GENERAL EN CONSUMO DE ATF", ex);
                    }
                    finally
                    {
                        LogsFact.Edit(log); // ACTUALIZAR ESTADO DEL LOG
                    }

                    if (log.Status == (int)LogStatus.ERRORSTATUS)
                        conn_error.Add(log.ConnID);
                }
            }
        }
        #endregion
    }
}