using System.Web.Mvc;

namespace ProfitTM.Areas.Compras
{
    public class ComprasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Compras";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Compras_default",
                "Compras/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ProfitTM.Areas.Compras.Controllers" }
            );
        }
    }
}