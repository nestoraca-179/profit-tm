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
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseAT = sqlController.getTypesCli();
                ProfitTMResponse responseAZ = sqlController.getZones();
                ProfitTMResponse responseAA = sqlController.getAccounts();
                ProfitTMResponse responseAC = sqlController.getCountries();
                ProfitTMResponse responseAS = sqlController.getSegments();
                List<Cond> conds = Cond.GetAllConds(connect);
                List<Seller> sellers = Seller.GetAllSellers(connect);

                responses.Add(responseAT);
                responses.Add(responseAZ);
                responses.Add(responseAA);
                responses.Add(responseAC);
                responses.Add(responseAS);

                foreach (ProfitTMResponse res in responses)
                {
                    if (res.Status == "ERROR")
                    {
                        error = true;
                        msg = res.Message;

                        break;
                    }
                }

                if (!error)
                {
                    ProfitTMResponse result;

                    ViewBag.results = option;
                    ViewBag.assistTypes = responseAT.Result;
                    ViewBag.assistZones = responseAZ.Result;
                    ViewBag.assistAccounts = responseAA.Result;
                    ViewBag.assistCountries = responseAC.Result;
                    ViewBag.assistSegments = responseAS.Result;
                    ViewBag.assistSellers = sellers;
                    ViewBag.assistConds = conds;

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
                else
                {
                    return RedirectToAction("Logout", "Account", new { area = "", msg = msg });
                }
            }
        }
    }
}