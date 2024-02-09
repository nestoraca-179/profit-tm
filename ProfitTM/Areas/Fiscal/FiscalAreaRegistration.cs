using System.Web.Mvc;

namespace ProfitTM.Areas.Fiscal
{
    public class FiscalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Fiscal";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Fiscal_default",
                "Fiscal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}