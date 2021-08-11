using System.Web.Mvc;

namespace ProfitTM.Areas.CajaBanco
{
    public class CajaBancoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CajaBanco";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CajaBanco_default",
                "CajaBanco/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ProfitTM.Areas.CajaBanco.Controllers" }
            );
        }
    }
}