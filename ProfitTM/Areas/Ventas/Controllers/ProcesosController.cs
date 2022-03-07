using ProfitTM.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class ProcesosController : Controller
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
                return View();
            }
        }

        public ActionResult Factura()
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
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = 50000000;

                ViewBag.invoices = serializer.Serialize(new Invoice().GetAllSaleInvoices());

                ViewBag.clients = new Client().GetAllClients();
                ViewBag.conds = new Cond().GetAllConds();
                ViewBag.sellers = new Seller().GetAllSellers();
                ViewBag.transports = new Transport().GetAllTransports();
                ViewBag.currencies = new Currency().GetAllCurrencies();

                return View();
            }
        }

        #region CODIGO ANTERIOR
        //public ActionResult EditarFactura(string id = "")
        //{
        //    ViewBag.user = Session["user"];
        //    ViewBag.modules = Session["modules"];

        //    if (ViewBag.user == null)
        //    {
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
        //    }
        //    else
        //    {
        //        string connect = Session["connect"].ToString();

        //        if (id != "")
        //        {
        //            /*Invoice order = Invoice.GetInvoice(connect, id, "V");
        //            ViewBag.order = order;*/
        //        }

        //        //ViewBag.clients = Client.GetAllClients(connect);
        //        ViewBag.conds = new Cond().GetAllConds();
        //        ViewBag.sellers = new Seller().GetAllSellers();
        //        ViewBag.transports = Transport.GetAllTransports(connect);
        //        ViewBag.currencies = Currency.GetAllCurrencies(connect);

        //        return View();
        //    }
        //}

        //public ActionResult ImprimirFactura(string id)
        //{
        //    ViewBag.user = Session["user"];
        //    ViewBag.modules = Session["modules"];

        //    ViewBag.report = id;

        //    if (ViewBag.user == null)
        //    {
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        //public ActionResult EditarPedido(string id = "")
        //{
        //    ViewBag.user = Session["user"];
        //    ViewBag.modules = Session["modules"];

        //    if (ViewBag.user == null)
        //    {
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
        //    }
        //    else
        //    {
        //        string connect = Session["connect"].ToString();

        //        if (id != "")
        //        {
        //            /*Invoice order = Invoice.GetInvoice(connect, id, "PV");
        //            ViewBag.order = order;*/
        //        }

        //        //ViewBag.clients = Client.GetAllClients(connect);
        //        ViewBag.conds = new Cond().GetAllConds();
        //        ViewBag.sellers = new Seller().GetAllSellers();
        //        ViewBag.transports = Transport.GetAllTransports(connect);
        //        ViewBag.currencies = Currency.GetAllCurrencies(connect);

        //        return View();
        //    }
        //}

        //public ActionResult ImportarPedido(string id = "")
        //{
        //    ViewBag.user = Session["user"];
        //    ViewBag.modules = Session["modules"];

        //    if (ViewBag.user == null)
        //    {
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
        //    }
        //    else
        //    {
        //        string connect = Session["connect"].ToString();

        //        if (id != "")
        //        {
        //            /*Invoice order = Invoice.GetInvoice(connect, id, "PV");
        //            ViewBag.order = order;*/
        //        }

        //        //ViewBag.clients = Client.GetAllClients(connect);
        //        ViewBag.conds = new Cond().GetAllConds();
        //        ViewBag.sellers = new Seller().GetAllSellers();
        //        ViewBag.transports = Transport.GetAllTransports(connect);
        //        ViewBag.currencies = Currency.GetAllCurrencies(connect);

        //        return View();
        //    }
        //}
        #endregion
    }
}