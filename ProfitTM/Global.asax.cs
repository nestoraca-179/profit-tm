using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using ProfitTM.Models;

namespace ProfitTM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Init()
        {
            Incident.CreateIncident("APPLICATION INIT", new Exception());
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
            //Incident.CreateIncident("INICIANDO QUARTZ", new Exception());
            ////var builder = Host.CreateDefaultBuilder().ConfigureServices((cxt, services) =>
            ////{
            ////    services.AddQuartz(q => {
            ////        q.UseMicrosoftDependencyInjectionJobFactory();
            ////    });

            ////    services.AddQuartzHostedService(opt => {
            ////        opt.WaitForJobsToComplete = true;
            ////    });

            ////}).Build();

            ////var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            //StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            //IScheduler scheduler = await schedulerFactory.GetScheduler();

            //var job = JobBuilder.Create<CerrarCajas>()
            //    .WithIdentity("myJob", "group1")
            //    .Build();

            //var trigger = TriggerBuilder.Create()
            //    .WithIdentity("myTrigger", "group1")
            //    .ForJob("myJob", "group1")
            //    .StartAt(DateTimeOffset.Now)
            //    .WithCronSchedule("0 55 0,1,2,3,4,17,18,19,20,23 ? * * *", x => x.WithMisfireHandlingInstructionIgnoreMisfires())
            //    //.WithCronSchedule("0 40 15 ? * * *", x => x.WithMisfireHandlingInstructionIgnoreMisfires())
            //    .Build();

            //await scheduler.ScheduleJob(job, trigger);
            //await scheduler.Start();
            ////await builder.RunAsync();

            //Incident.CreateIncident("QUARTZ ACTIVADO " + scheduler.IsStarted, new Exception());
            //Incident.CreateIncident("QUARTZ APAGADO " + scheduler.IsShutdown, new Exception());
            //Incident.CreateIncident("QUARTZ EN STAND BY " + scheduler.InStandbyMode, new Exception());
            //Incident.CreateIncident("FINALIZANDO QUARTZ " + trigger.StartTimeUtc.DateTime.ToString("dd/MM/yyyy HH:mm:ss"), new Exception());
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
            Incident.CreateIncident("APPLICATION DISPOSED", new Exception());
        }

        protected void Application_End()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.RemoveAll();

            Incident.CreateIncident("APPLICATION END", new Exception());
        }

        protected void Session_Start()
        {
            Incident.CreateIncident("SESSION START", new Exception());
        }

        protected void Session_End()
        {
            Incident.CreateIncident("SESSION END", new Exception());
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        #region QUARTZ
        //public class CerrarCajas : IJob
        //{
        //    Task IJob.Execute(IJobExecutionContext context)
        //    {
        //        try
        //        {
        //            Incident.CreateIncident("INICIANDO CERRAR CAJAS", new Exception());
        //            Box.CloseAllBoxes();
        //            Incident.CreateIncident("FINALIZANDO CERRAR CAJAS", new Exception());
        //        }
        //        catch (Exception ex)
        //        {
        //            Incident.CreateIncident("ERROR CERRANDO CAJAS", ex);
        //        }

        //        return Task.CompletedTask;
        //    }
        //}
        #endregion
    }
}