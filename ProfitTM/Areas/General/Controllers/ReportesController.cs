using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.General.Controllers
{
    [Authorize]
    public class ReportesController : Controller
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

                return View();
            }
        }

        public ActionResult Reporte(string name = "", string format = "")
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

                ViewBag.name = name;
                ViewBag.report = format;

                return View();
            }
        }
    }
}