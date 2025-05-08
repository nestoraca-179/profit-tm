using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using ProfitTM.Models;

namespace ProfitTM.Areas.CajaBanco.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        public ActionResult Index()
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

                return View();
            }
        }

        public ActionResult Caja()
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
                ViewBag.sucur = Session["BRANCH"];
                ViewBag.modules = Session["MODULES"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.product = "Administrativo";
                ViewBag.username = (Session["USER"] as Users).Username;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;

                // if (Session["BENEFS"] == null)
                //     Session["BENEFS"] = serializer.Serialize(new Beneficiary().GetAllBeneficiaries());

                if (Session["ACCOUNTS"] == null)
                    Session["ACCOUNTS"] = serializer.Serialize(new Account().GetAllAccounts());

                if (Session["SUPPLIERS"] == null)
                    Session["SUPPLIERS"] = serializer.Serialize(new Supplier().GetAllSuppliers());

                ViewBag.benefs = serializer.Serialize(new Beneficiary().GetAllBeneficiaries());
                ViewBag.accounts = Session["ACCOUNTS"];
                ViewBag.suppliers = Session["SUPPLIERS"];

                return View();
            }
        }
    
        public ActionResult ConciliacionM()
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
                int conn = int.Parse(Session["ID_CONN"].ToString());

                ViewBag.user = Session["USER"];
                ViewBag.sucur = Session["BRANCH"];
                ViewBag.modules = Session["MODULES"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.product = "Administrativo";
                ViewBag.username = (Session["USER"] as Users).Username;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;
                ViewBag.transfers = serializer.Serialize(Transfer.GetAllTransfers(conn));

                return View();
            }
        }

        public ActionResult ImprimirPago(string id)
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
                ViewBag.pag = id;

                return View();
            }
        }
    
        public ActionResult ImprimirOrdenPago(string id)
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
                ViewBag.ord = id;

                return View();
            }
        }
    }
}