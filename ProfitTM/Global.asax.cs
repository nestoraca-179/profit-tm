using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Threading.Tasks;
using System.Web.SessionState;
using ProfitTM.Models;
using Quartz;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ProfitTM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected async void Application_Start()
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

            Incident.CreateIncident("INICIANDO QUARTZ", new Exception());
            var builder = Host.CreateDefaultBuilder().ConfigureServices((cxt, services) =>
            {
                services.AddQuartz(q => {
                    q.UseMicrosoftDependencyInjectionJobFactory();
                });

                services.AddQuartzHostedService(opt => {
                    opt.WaitForJobsToComplete = true;
                });

            }).Build();

            var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            var job = JobBuilder.Create<CerrarCajas>()
                .WithIdentity("myJob", "group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithCronSchedule("0 55 23 * * *")
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await builder.RunAsync();
            Incident.CreateIncident("FINALIZANDO QUARTZ", new Exception());
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
            Task IJob.Execute(IJobExecutionContext context)
            {
                try
                {
                    Incident.CreateIncident("INICIANDO CERRAR CAJAS", new Exception());
                    Box.CloseAllBox();
                    Incident.CreateIncident("FINALIZANDO CERRAR CAJAS", new Exception());
                }
                catch (Exception ex)
                {
                    Incident.CreateIncident("ERROR CERRANDO CAJAS", ex);
                }

                return Task.CompletedTask;
            }
        }
    }
}