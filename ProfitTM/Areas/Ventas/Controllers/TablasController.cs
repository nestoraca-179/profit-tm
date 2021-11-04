using ProfitTM.Controllers;
using ProfitTM.Enum;
using ProfitTM.Models;
using System.Collections.Generic;
using System.Globalization;
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
            ViewBag.options = Session["options"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
            }
            else
            {
                string connect = Session["connect"].ToString();

                List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

                if (function == EnumLoadFunction.CLIENT)
                {
                    List<Client> clients = Client.GetAllClients(connect);

                    if (clients != null)
                    {
                        foreach (Client client in clients)
                        {
                            Dictionary<string, string> item = new Dictionary<string, string>();

                            item.Add("ID", client.ID);
                            item.Add("RIF", client.RIF);
                            item.Add("Client", client.Name);
                            item.Add("Address", client.Address);
                            item.Add("Phone", client.Phone);
                            item.Add("Email", client.Email);

                            results.Add(item);
                        }

                        ViewBag.function = "Cliente";
                        ViewBag.headers = "Codigo,RIF,Nombre,Direccion,Telefono,Email";
                        ViewBag.cols = "ID,RIF,Client,Address,Phone,Email";
                        ViewBag.resultsTable = results;
                    }
                    else
                    {
                        ViewBag.errorMessage = "Error cargando documentos";
                        ViewBag.results = "";
                    }
                }
                else
                {
                    ViewBag.results = "";
                }

                ViewBag.types = Type.GetAllTypesAdmin(connect, "C");
                ViewBag.zones = Zone.GetAllZones(connect);
                ViewBag.accounts = Account.GetAllAccounts(connect);
                ViewBag.countries = Country.GetAllCountries(connect);
                ViewBag.segments = Segment.GetAllSegments(connect);
                ViewBag.sellers = Seller.GetAllSellers(connect);
                ViewBag.conds = Cond.GetAllConds(connect);

                return View();
            }
        }
    
        public ActionResult EditarCliente(string id = "")
        {
            ViewBag.user = Session["user"];
            ViewBag.options = Session["options"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
            }
            else
            {
                return View();
            }
        }
    }
}