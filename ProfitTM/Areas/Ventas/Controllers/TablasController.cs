using ProfitTM.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
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
    
        public ActionResult Cliente()
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

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;

                ViewBag.clients = serializer.Serialize(new Client().GetAllClients());

                ViewBag.conds = new Cond().GetAllConds();
                ViewBag.sellers = new Seller().GetAllSellers();
                ViewBag.zones = new Zone().GetAllZones();
                ViewBag.accounts = new Account().GetAllAccounts();
                ViewBag.countries = new Country().GetAllCountries();
                ViewBag.segments = new Segment().GetAllSegments();
                ViewBag.types = new TypePerson().GetAllTypeClients();

                return View();
            }
        }
    }
}