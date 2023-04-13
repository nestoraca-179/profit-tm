using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using ProfitTM.Models;
using Quartz;
using Quartz.Impl;

namespace ProfitTM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
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

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            IJobDetail jobCerrarCajas = JobBuilder.Create<CerrarCajas>().Build();
            ITrigger triggerCerrarCajas = TriggerBuilder.Create()
                .WithIdentity("triggerCerrarCajas")
                .StartNow()
                .WithCronSchedule("0 55 23 * * ?")
                .Build();

            scheduler.ScheduleJob(jobCerrarCajas, triggerCerrarCajas);
            scheduler.Start();
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

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        public class CerrarCajas : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                try
                {
                    Box.CloseAllBox();
                }
                catch (Exception ex)
                {
                    Incident.CreateIncident("ERROR CERRANDO CAJAS", ex);
                }
            }
        }
    }
}