using ProfitTM.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
{
    [Authorize]
    public class TablasController : Controller
    {
        public ActionResult Index(string function = "")
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
                string connect = Session["connect"].ToString();
                List<Client> clients = Client.GetAllClients(connect);

                ViewBag.clients = clients;

                ViewBag.conds = Cond.GetAllConds(connect);
                ViewBag.sellers = Seller.GetAllSellers(connect);
                ViewBag.zones = Zone.GetAllZones(connect);
                ViewBag.accounts = Account.GetAllAccounts(connect);
                ViewBag.countries = Country.GetAllCountries(connect);
                ViewBag.segments = Segment.GetAllSegments(connect);
                ViewBag.types = TypePerson.GetAllTypesAdmin(connect, "C");

                return View();
            }
        }
    }
}