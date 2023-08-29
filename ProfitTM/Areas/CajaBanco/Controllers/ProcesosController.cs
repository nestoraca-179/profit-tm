using System.Web.Mvc;
using System.Web.Script.Serialization;
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

                ViewBag.benefs = serializer.Serialize(new Beneficiary().GetAllBeneficiaries());
                ViewBag.accounts = Session["ACCOUNTS"];

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
                ViewBag.user = Session["USER"];
                ViewBag.sucur = Session["BRANCH"];
                ViewBag.modules = Session["MODULES"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.product = "Administrativo";
                ViewBag.username = (Session["USER"] as Users).Username;

                ViewBag.transfers = Transfer.GetAllTransfers();

                return View();
            }
        }
    }
}