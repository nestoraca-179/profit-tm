using ProfitTM.Controllers;
using ProfitTM.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
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
                string connect = Session["connect"].ToString();
                SQLController sqlController = new SQLController();

                ProfitTMResponse result;

                ViewBag.results = option;
                ViewBag.assistTypes = Type.GetAllTypesAdmin(connect, "C");
                ViewBag.assistZones = Zone.GetAllZones(connect);
                ViewBag.assistAccounts = Account.GetAllAccounts(connect);
                ViewBag.assistCountries = Country.GetAllCountries(connect);
                ViewBag.assistSegments = Segment.GetAllSegments(connect);
                ViewBag.assistSellers = Seller.GetAllSellers(connect);
                ViewBag.assistConds = Cond.GetAllConds(connect); ;

                switch (option)
                {
                    case "0":

                        result = sqlController.getResultsTable("co_cli,rif,cli_des,direc1,telefonos,email", "saCliente");

                        if (result.Status == "OK")
                        {
                            ViewBag.resultsTable = result.Result;
                            ViewBag.titleR = "Cliente";
                            ViewBag.headers = "Codigo,RIF,Nombre,Direccion,Telefono,Email";
                            ViewBag.cols = "co_cli,rif,cli_des,direc1,telefonos,email";
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