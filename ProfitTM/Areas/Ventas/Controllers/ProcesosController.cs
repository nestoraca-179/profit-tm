using ProfitTM.Controllers;
using ProfitTM.Models;
using ProfitTM.Enum;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class ProcesosController : Controller
    {
        public ActionResult Index(string option = "", string function = "")
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
                string msg = "", connect = Session["connect"].ToString();

                ProfitTMResponse responseAC = sqlController.getClients();
                ProfitTMResponse responseAN = sqlController.getConds();
                ProfitTMResponse responseAS = sqlController.getSellers();
                List<Transport> transports = Transport.GetAllTransports(connect);

                responses.Add(responseAC);
                responses.Add(responseAN);
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
                    List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

                    NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                    formato.CurrencyGroupSeparator = ".";
                    formato.NumberDecimalSeparator = ",";

                    if (function == EnumLoadFunction.INVOICEV)
                    {
                        List<Invoice> invoices = Invoice.GetAllInvoices(connect, "V");
                        List<Invoice> templates = Invoice.GetAllInvoices(connect, "PV");
                        List<Modal> modals = Option.GetModals(option);

                        if (invoices != null)
                        {
                            foreach (Invoice invoice in invoices)
                            {
                                Dictionary<string, string> item = new Dictionary<string, string>();

                                item.Add("ID", invoice.ID);
                                item.Add("cli_des", invoice.InvoicePerson.Name);
                                item.Add("fec_emis", invoice.Date.ToString("dd/MM/yyyy HH:mm tt"));
                                item.Add("total_neto", invoice.Amount.ToString("N", formato));

                                switch (invoice.Status)
                                {
                                    case 0:
                                        item.Add("status", "NO PROCESADA");
                                        break;
                                    case 1:
                                        item.Add("status", "PARCIALMENTE PROCESADA");
                                        break;
                                    case 2:
                                        item.Add("status", "PROCESADA");
                                        break;
                                }

                                if (invoice.Printed)
                                    item.Add("impresa", "SI");
                                else
                                    item.Add("impresa", "NO");

                                if (invoice.Status == 0 && !invoice.Printed)
                                {
                                    item.Add("edit", "True");
                                    item.Add("delete", "True");
                                }
                                else
                                {
                                    item.Add("edit", "False");
                                    item.Add("delete", "False");
                                }

                                string fieldsEdit = string.Format(
                                    "idFacturaEdit={0},personFacturaEdit={1},condFacturaEdit={2},sellerFacturaEdit={3},controlFacturaEdit={4},transFacturaEdit={5},montoFacturaEdit={6},tipoFacturaEdit=V",
                                    invoice.ID,
                                    invoice.InvoicePerson.ID,
                                    invoice.InvoicePerson.Cond.ID,
                                    invoice.InvoicePerson.Seller.ID,
                                    invoice.ControlNumber,
                                    invoice.Transport.ID,
                                    invoice.Amount.ToString().Replace(",", ".")
                                );

                                item.Add("details", "True");
                                item.Add("fieldsEdit", fieldsEdit);

                                results.Add(item);
                            }

                            ViewBag.modals = modals;
                            ViewBag.documents = invoices;
                            ViewBag.templates = templates;
                            ViewBag.resultsTable = results;

                            ViewBag.titleR = "Factura";
                            ViewBag.headers = "Codigo,Cliente,Fecha,Total,Estado,Impresa";
                            ViewBag.cols = "ID,cli_des,fec_emis,total_neto,status,impresa";
                        }
                        else
                        {
                            ViewBag.errorMessage = "Error cargando facturas";
                            ViewBag.results = "";
                        }
                    }
                    else if (function == EnumLoadFunction.INVOICEPV)
                    {
                        List<Invoice> invoices = Invoice.GetAllInvoices(connect, "V");
                        List<Modal> modals = Option.GetModals(option);

                        if (invoices != null)
                        {
                            foreach (Invoice invoice in invoices)
                            {
                                Dictionary<string, string> item = new Dictionary<string, string>();

                                item.Add("ID", invoice.ID);
                                item.Add("cli_des", invoice.InvoicePerson.ID);
                                item.Add("fec_emis", invoice.Date.ToString());
                                item.Add("total_neto", invoice.Amount.ToString("N", formato));

                                switch (invoice.Status)
                                {
                                    case 0:
                                        item.Add("status", "NO PROCESADA");
                                        break;
                                    case 1:
                                        item.Add("status", "PARCIALMENTE PROCESADA");
                                        break;
                                    case 2:
                                        item.Add("status", "PROCESADA");
                                        break;
                                }

                                if (invoice.Printed)
                                    item.Add("impresa", "SI");
                                else
                                    item.Add("impresa", "NO");

                                if (invoice.Status == 0 && !invoice.Printed)
                                {
                                    item.Add("edit", "True");
                                    item.Add("delete", "True");
                                }
                                else
                                {
                                    item.Add("edit", "False");
                                    item.Add("delete", "False");
                                }

                                string fieldsEdit = string.Format(
                                    "idFacturaEdit={0},personFacturaEdit={1},condFacturaEdit={2},sellerFacturaEdit={3},controlFacturaEdit={4},transFacturaEdit={5},montoFacturaEdit={6},tipoFacturaEdit=PV",
                                    invoice.ID,
                                    invoice.InvoicePerson.ID,
                                    invoice.InvoicePerson.Cond.ID,
                                    invoice.InvoicePerson.Seller.ID,
                                    invoice.ControlNumber,
                                    invoice.Transport.ID,
                                    invoice.Amount.ToString().Replace(",", ".")
                                );

                                item.Add("details", "True");
                                item.Add("fieldsEdit", fieldsEdit);

                                results.Add(item);
                            }

                            ViewBag.modals = modals;
                            ViewBag.documents = invoices;
                            ViewBag.resultsTable = results;

                            ViewBag.titleR = "Plantilla";
                            ViewBag.headers = "Codigo,Cliente,Fecha,Total,Estado,Impresa";
                            ViewBag.cols = "ID,cli_des,fec_emis,total_neto,status,impresa";
                        }
                        else
                        {
                            ViewBag.errorMessage = "Error cargando facturas";
                            ViewBag.results = "";
                        }
                    }
                    else
                    {
                        ViewBag.results = "";
                    }

                    ViewBag.assistClients = responseAC.Result;
                    ViewBag.assistConds = responseAN.Result;
                    ViewBag.assistSellers = responseAS.Result;
                    ViewBag.assistTransports = transports;
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