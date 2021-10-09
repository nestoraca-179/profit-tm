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
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseAT = sqlController.getTypesSup();
                ProfitTMResponse responseAZ = sqlController.getZones();
                ProfitTMResponse responseAA = sqlController.getAccounts();
                ProfitTMResponse responseAC = sqlController.getCountries();
                ProfitTMResponse responseAS = sqlController.getSegments();

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

                    switch (option)
                    {
                        case "0":

                            result = sqlController.getResultsTable("co_prov,rif,prov_des,direc1,telefonos,email", "saProveedor");

                            if (result.Status == "OK")
                            {
                                ViewBag.resultsTable = result.Result;
                                ViewBag.titleR = "Proveedor";
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
                else
                {
                    return RedirectToAction("Logout", "Account", new { area = "", msg = msg });
                }
            }
        }
    }
}