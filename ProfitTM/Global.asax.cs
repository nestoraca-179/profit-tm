using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using ProfitTM.Models;
using Quartz;
using Quartz.Impl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Collections.Generic;

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
                try
                {
                    var httpContext = context.MergedJobDataMap.Get("HttpContext") as HttpContextBase; // REVISAR
                    if (httpContext != null)
                    {
                        string userName = httpContext.Session["ID_CONN"] as string;
                        if (!string.IsNullOrEmpty(userName))
                        {
                            Console.WriteLine($"El usuario {userName} ha iniciado sesión.");
                        }
                    }

                    string id_conn = HttpContext.Current != null ? HttpContext.Current.Session["ID_CONN"].ToString() : "";
                    if (!string.IsNullOrEmpty(id_conn))
                    {
                        Connections conn = Connection.GetConnByID(id_conn);
                        if (conn.UseFactOnline)
                        {
                            if (conn.Token == null || conn.DateToken == null || DateTime.Now > conn.DateToken)
                            {
                                ModelAuth auth = new ModelAuth() { usuario = conn.UserToken, clave = conn.PassToken };
                                ModelResponse response = await new Root().SendAuth(auth);
                                
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

                            List<LogsFactOnline> logs = LogsFact.GetPendingLogs();
                            foreach (LogsFactOnline log in logs)
                            {
                                saFacturaVenta i = new Invoice().GetSaleInvoiceByID(log.NroFact);
                                Root json = new Root().GetInvoiceInfo(i);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    int i = 0;
                }
            }
        }
        #endregion
    }
}