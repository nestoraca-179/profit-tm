using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class TablasController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            ViewBag.modules = Session["modules"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { area = "", msg = "Debes elegir una empresa" });
            }
            else
            {
                ViewBag.user_conn = Session["user_conn"].ToString();
                ViewBag.data_conn = Session["data_conn"].ToString();
                ViewBag.bran_conn = Session["bran_conn"].ToString();

                return View();
            }
        }
    }
}