using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.CajaBanco.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user = Session["USER"];
            ViewBag.connect = Session["CONNECT"];
            ViewBag.modules = Session["MODULES"];
            ViewBag.product = "Administrativo";

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
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();

                return View();
            }
        }

        public ActionResult Reporte(string name = "", string format = "")
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
            }
            else if (Session["CONNECT"] == null)
            {
                return RedirectToAction("Logout", "Account", new { area = "", msg = "Debes elegir una empresa" });
            }
            else
            {
                ViewBag.user = Session["USER"];
                ViewBag.modules = Session["MODULES"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.product = "Administrativo";

                ViewBag.name = name;
                ViewBag.report = format;

                return View();
            }
        }
    }
}