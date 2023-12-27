using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using ProfitTM.Models;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.product = "Administrativo";
            ViewBag.data_conn = Session["DATA_CONN"].ToString();
            ViewBag.bran_conn = Session["BRAN_CONN"].ToString();

            return View();
        }

        public ActionResult OrdenCompra()
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
                Users user = Session["USER"] as Users;
                string sucur = Session["BRANCH"].ToString();
                int conn = int.Parse(Session["ID_CONN"].ToString());

                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.username = user.Username;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;

                ViewBag.orders = serializer.Serialize(new BuyOrder().GetAllBuyOrders(200));

                if (Session["ARTS_C"] == null)
                    Session["ARTS_C"] = serializer.Serialize(new Product().GetAllNameArts(false));

                if (Session["SUPPLIERS"] == null)
                    Session["SUPPLIERS"] = serializer.Serialize(new Supplier().GetAllSuppliers());

                if (Session["CURRENCIES"] == null)
                    Session["CURRENCIES"] = serializer.Serialize(new Currency().GetAllCurrencies());

                if (Session["CONDS"] == null)
                    Session["CONDS"] = serializer.Serialize(new Cond().GetAllConds());

                ViewBag.arts = Session["ARTS_C"];
                ViewBag.suppliers = Session["SUPPLIERS"];
                ViewBag.currencies = Session["CURRENCIES"];
                ViewBag.conds = Session["CONDS"];

                return View();
            }
        }
    }
}