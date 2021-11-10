using ProfitTM.Controllers;
using ProfitTM.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class TablasController : Controller
    {
        public ActionResult Index(string option = "")
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
                SQLController sqlController = new SQLController();
                string connect = Session["connect"].ToString();

                ProfitTMResponse result;

                ViewBag.results = option;
                ViewBag.types = TypePerson.GetAllTypesAdmin(connect, "P");
                ViewBag.zones = Zone.GetAllZones(connect);
                ViewBag.accounts = Account.GetAllAccounts(connect);
                ViewBag.countries = Country.GetAllCountries(connect);
                ViewBag.segments = Segment.GetAllSegments(connect);

                switch (option)
                {
                    case "0":

                        result = sqlController.getResultsTable("co_prov,rif,prov_des,direc1,telefonos,email", "saProveedor");

                        if (result.Status == "OK")
                        {
                            ViewBag.resultsTable = result.Result;
                            ViewBag.function = "Proveedor";
                            ViewBag.headers = "Codigo,RIF,Nombre,Direccion,Telefono,Email";
                            ViewBag.cols = "co_prov,rif,prov_des,direc1,telefonos,email";
                        }

                        break;
                    default:
                        ViewBag.results = "";
                        break;
                }

                return View();
            }
        }
    }
}