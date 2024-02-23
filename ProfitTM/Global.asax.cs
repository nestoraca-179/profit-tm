using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using ProfitTM.Models;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProfitTM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
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
            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = schedulerFactory.GetScheduler().Result;

            var job = JobBuilder.Create<PruebaJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
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
        public class PruebaJob : IJob
        {
            async Task IJob.Execute(IJobExecutionContext context)
            {
                List<LogsFactOnline> logs = LogsFact.GetPendingLogs();
                foreach (LogsFactOnline log in logs)
                {
                    try
                    {
                        Connections conn = Connection.GetConnByID(log.ConnID.ToString());
                        if (conn.Token == null || conn.DateToken == null || DateTime.Now > conn.DateToken)
                        {
                            ModelAuthRequest auth = new ModelAuthRequest() { usuario = conn.UserToken, clave = conn.PassToken };
                            ModelAuthResponse response = await new Root().SendAuth(auth);

                            if (response.codigo == 200)
                            {
                                conn.Token = response.token;
                                conn.DateToken = response.expiracion.AddHours(-4);
                                Connection.Edit(conn);
                            }
                            else
                            {
                                throw new Exception(response.mensaje);
                            }
                        }

                        log.Times++;
                        log.DateTried = DateTime.Now;
                        ModelInvoiceInfoResponse info = await new Root().SendInvoiceInfoAsync(log.BodyJson, conn.Token);

                        if (info.codigo == "200")
                        {
                            log.Status = 1; // SENT
                            log.DateSent = DateTime.Now;
                            log.NroControl = info.resultado.numeroControl;
                            log.Message = "OK";
                        }
                        else if (info.codigo == "203")
                        {
                            ModelAssignRequest assign = new ModelAssignRequest()
                            {
                                detalleAsignacion = new List<DetalleAsignacion>()
                                {
                                    new DetalleAsignacion()
                                    {
                                        serie = "A",
                                        tipoDocumento = "01",
                                        numeroDocumentoInicio = log.NroFact,
                                        numeroDocumentoFin = log.NroFact
                                    }
                                }
                            };
                            ModelAssignResponse response = await new Root().SendAssign(assign);

                            log.Status = 3; // WAITING
                            log.Message = "WAITING FOR RE-SEND";
                        }
                    }
                    catch (AuthenticationException ex)
                    {
                        log.Status = 2; // ERROR AUTHENTICATION
                        log.Message = ex.Message;
                        log.HttpCode = ex.Message.Split(new string[] { " ** " }, StringSplitOptions.RemoveEmptyEntries)[1];

                        Incident.CreateIncident("ERROR EN AUTENTICACION DE ATF", ex);
                    }
                    catch (InformationException ex)
                    {
                        log.Status = 2; // ERROR INFORMATION
                        log.Message = ex.Message;
                        log.HttpCode = ex.Message.Split(new string[] { " ** " }, StringSplitOptions.RemoveEmptyEntries)[1];

                        Incident.CreateIncident("ERROR EN INFORMACION DE ATF", ex);
                    }
                    catch (AssignmentException ex)
                    {
                        log.Status = 2; // ERROR ASSIGNMENT
                        log.Message = ex.Message;
                        log.HttpCode = ex.Message.Split(new string[] { " ** " }, StringSplitOptions.RemoveEmptyEntries)[1];

                        Incident.CreateIncident("ERROR EN ASIGNACION DE ATF", ex);
                    }
                    catch (Exception ex)
                    {
                        log.Status = 2; // ERROR GENERAL
                        log.Message = ex.Message;

                        Incident.CreateIncident("ERROR GENERAL EN CONSUMO DE ATF", ex);
                    }
                    finally
                    {
                        LogsFact.Edit(log); // ACTUALIZAR ESTADO DEL LOG
                    }

                    if (log.Status != 1 || log.Status != 3)
                        break;
                }
            }
        }
        #endregion
    }
}