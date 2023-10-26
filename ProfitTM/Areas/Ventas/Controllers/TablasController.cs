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
    
        public ActionResult Cliente()
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

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 100000000;

                ViewBag.clients = serializer.Serialize(new Client().GetAllClients(true));

                if (Session["CONDS"] == null)
                    Session["CONDS"] = serializer.Serialize(new Cond().GetAllConds());

                if (Session["SELLERS"] == null)
                    Session["SELLERS"] = serializer.Serialize(new Seller().GetAllSellers());

                if (Session["ZONES"] == null)
                    Session["ZONES"] = serializer.Serialize(new Zone().GetAllZones());

                if (Session["ACCOUNTS"] == null)
                    Session["ACCOUNTS"] = serializer.Serialize(new Account().GetAllAccounts());

                if (Session["COUNTRIES"] == null)
                    Session["COUNTRIES"] = serializer.Serialize(new Country().GetAllCountries());

                if (Session["SEGMENTS"] == null)
                    Session["SEGMENTS"] = serializer.Serialize(new Segment().GetAllSegments());

                if (Session["TYPES"] == null)
                    Session["TYPES"] = serializer.Serialize(new TypePerson().GetAllTypeClients());

                ViewBag.conds = Session["CONDS"];
                ViewBag.sellers = Session["SELLERS"];
                ViewBag.zones = Session["ZONES"];
                ViewBag.accounts = Session["ACCOUNTS"];
                ViewBag.countries = Session["COUNTRIES"];
                ViewBag.segments = Session["SEGMENTS"];
                ViewBag.types = Session["TYPES"];

                return View();
            }
        }

        public ActionResult ConsultarCliente()
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
                ViewBag.username = (Session["USER"] as Users).Username;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;

                if (Session["CLIENTS"] == null)
                    Session["CLIENTS"] = serializer.Serialize(new Client().GetAllClients(false));

                ViewBag.clients = Session["CLIENTS"];

                return View();
            }
        }
    }
}