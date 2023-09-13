using System.Web.Mvc;
using System.Web.Security;
using System.Web.Script.Serialization;
using ProfitTM.Models;

namespace ProfitTM.Areas.Ventas.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
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

        public ActionResult Factura()
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

                int id;

                if ((sucur == "002" || sucur == "003" || sucur == "004" || sucur == "005") && user.UseBox && user.AllowCollect)
                    id = Box.GetBoxOpenByUser(user.Username, conn, user.BoxType.Value, false);
                else
                    id = -1;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;
                
                ViewBag.invoices = serializer.Serialize(new Invoice().GetAllSaleInvoices(200, sucur));

                if (Session["ARTS"] == null)
                    Session["ARTS"] = serializer.Serialize(new Product().GetAllNameSellArts());

                if (Session["CONDS"] == null)
                    Session["CONDS"] = serializer.Serialize(new Cond().GetAllConds());

                // if (Session["CLIENTS"] == null)
                //     Session["CLIENTS"] = serializer.Serialize(new Client().GetAllClients(false));

                if (Session["CURRENCIES"] == null)
                    Session["CURRENCIES"] = serializer.Serialize(new Currency().GetAllCurrencies());

                if (Session["SELLERS"] == null)
                    Session["SELLERS"] = serializer.Serialize(new Seller().GetAllSellers());

                if (Session["BANKACCOUNTS"] == null)
                    Session["BANKACCOUNTS"] = serializer.Serialize(new Account().GetAllBankAccounts());

                if (Session["TEMPLATE"] == null)
                    Session["TEMPLATE"] = serializer.Serialize(new Invoice().GetTemplate());

                ViewBag.id_box = id;
                ViewBag.arts = Session["ARTS"];
                ViewBag.conds = Session["CONDS"];
                // ViewBag.clients = Session["CLIENTS"];
                ViewBag.currencies = Session["CURRENCIES"];
                ViewBag.sellers = Session["SELLERS"];
                ViewBag.bankAccounts = Session["BANKACCOUNTS"];
                ViewBag.template = Session["TEMPLATE"];

                return View();
            }
        }

        public ActionResult Cobro()
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
                string sucur = Session["BRANCH"].ToString();
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.username = (Session["USER"] as Users).Username;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;

                ViewBag.collects = serializer.Serialize(new Collect().GetAllCollects(200, sucur));

                if (Session["CLIENTS"] == null)
                    Session["CLIENTS"] = serializer.Serialize(new Client().GetAllClients(false));

                if (Session["CURRENCIES"] == null)
                    Session["CURRENCIES"] = serializer.Serialize(new Currency().GetAllCurrencies());

                if (Session["SELLERS"] == null)
                    Session["SELLERS"] = serializer.Serialize(new Seller().GetAllSellers());

                ViewBag.clients = Session["CLIENTS"];
                ViewBag.currencies = Session["CURRENCIES"];
                ViewBag.sellers = Session["SELLERS"];

                return View();
            }
        }
    
        public ActionResult Preliquidacion()
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
                serializer.MaxJsonLength = 50000000;

                ViewBag.orders = serializer.Serialize(new Order().GetAllOrders(200, false));
                ViewBag.arts = serializer.Serialize(new Product().GetAllArts());

                ViewBag.clients = new Client().GetAllClients(false);
                ViewBag.conds = new Cond().GetAllConds();
                ViewBag.sellers = new Seller().GetAllSellers();
                ViewBag.obj_client = serializer.Serialize(ViewBag.clients);

                return View();
            }
        }

        public ActionResult ImprimirFactura(string id, string type)
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
                ViewBag.format = type == "002" ? "RepFormatoFacturaVentaOMPartial" : "RepFormatoFacturaVentaPartial";
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.fact = id;

                return View();
            }
        }
    }
}