using ProfitTM.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Controllers
{
    public class ConexionesController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            ViewBag.modules = Session["modules"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                switch (Session["prod"].ToString())
                {
                    case "ADM":
                        ViewBag.product = "Administrativo";
                        break;
                    case "CON":
                        ViewBag.product = "Contabilidad";
                        break;
                    case "NOM":
                        ViewBag.product = "Nómina";
                        break;
                }
                
                ViewBag.data_conn = Session["data_conn"].ToString();
                ViewBag.bran_conn = Session["bran_conn"] != null ? Session["bran_conn"].ToString() : null;

                ViewBag.connections = Connection.GetConnectionsByType();
                ViewBag.prod = Session["prod"].ToString();

                return View();
            }
        }
    }
}