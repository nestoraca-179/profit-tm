using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.General.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        public ActionResult Index(string name = "", string format = "")
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
                ViewBag.name = name;
                ViewBag.report = format;

                return View();
            }
        }
    }
}