using ProfitTM.Controllers;
using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class ProcesosController : Controller
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
                InvoiceManager invoiceManager = new InvoiceManager();
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseAC = sqlController.getClients();

                responses.Add(responseAC);

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

                    ViewBag.assistClients = responseAC.Result;

                    switch (option)
                    {
                        case "0":

                            result = invoiceManager.getAllInvoices('V');

                            if (result.Status == "OK")
                            {
                                ViewBag.resultsTable = result.Result;
                                ViewBag.titleR = "Factura";
                                ViewBag.headers = "Codigo,Cliente,Fecha,Total,Estado,Impresa";
                            }

                            break;
                        default:
                            ViewBag.results = "";
                            break;
                    }

                    NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                    formato.CurrencyGroupSeparator = ".";
                    formato.NumberDecimalSeparator = ",";
                    ViewBag.formato = formato;

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