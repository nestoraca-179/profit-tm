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
                string connect = Session["connect"].ToString();

                List<Client> clients = Client.GetAllClients(connect);
                List<Cond> conds = Cond.GetAllConds(connect);
                List<Seller> sellers = Seller.GetAllSellers(connect);
                List<Transport> transports = Transport.GetAllTransports(connect);
                List<Currency> currencies = Currency.GetAllCurrencies(connect);

                List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

                NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                formato.CurrencyGroupSeparator = ".";
                formato.NumberDecimalSeparator = ",";

                if (function == EnumLoadFunction.INVOICEV || function == EnumLoadFunction.INVOICEPV)
                {
                    string type = function == EnumLoadFunction.INVOICEV ? "V" : "PV";

                    List<Invoice> invoices = Invoice.GetAllInvoices(connect, type), templates = Invoice.GetAllInvoices(connect, "PV");
                    List<Modal> modals = Option.GetModals(option);

                    if (invoices != null && templates != null)
                    {
                        foreach (Invoice invoice in invoices)
                        {
                            Dictionary<string, string> item = new Dictionary<string, string>();

                            item.Add("ID", invoice.ID);
                            item.Add("PersonName", invoice.InvoicePerson.Name);
                            item.Add("DateEmis", invoice.DateEmis.ToString("dd/MM/yyyy HH:mm tt"));
                            item.Add("Amount", invoice.Amount.ToString("N", formato));

                            switch (invoice.Status)
                            {
                                case 0:
                                    item.Add("Status", "NO PROCESADA");
                                    break;
                                case 1:
                                    item.Add("Status", "PARCIALMENTE PROCESADA");
                                    break;
                                case 2:
                                    item.Add("Status", "PROCESADA");
                                    break;
                            }

                            if (invoice.Printed)
                                item.Add("Impresa", "SI");
                            else
                                item.Add("Impresa", "NO");

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
                                "idFacturaEdit={0},personFacturaEdit={1},condFacturaEdit={2},sellerFacturaEdit={3},controlFacturaEdit={4},transFacturaEdit={5},montoFacturaEdit={6},monedaFacturaEdit={7},dateFacturaEdit={8},tipoFacturaEdit=V",
                                invoice.ID,
                                invoice.InvoicePerson.ID,
                                invoice.InvoicePerson.Cond.ID,
                                invoice.InvoicePerson.Seller.ID,
                                invoice.ControlNumber,
                                invoice.Transport.ID,
                                invoice.Amount.ToString().Replace(",", "."),
                                invoice.Currency.ID,
                                invoice.DateEmis.ToString("yyyy/MM/dd HH:mm")
                            );

                            item.Add("details", "True");
                            item.Add("fieldsEdit", fieldsEdit);

                            results.Add(item);
                        }

                        ViewBag.modals = modals;
                        ViewBag.documents = invoices;
                        ViewBag.templates = templates;
                        ViewBag.resultsTable = results;

                        if (type == "V")
                            ViewBag.titleR = "Factura";
                        else
                            ViewBag.titleR = "Plantilla";

                        ViewBag.headers = "Codigo,Cliente,Fec. Emis,Total,Estado,Impresa";
                        ViewBag.cols = "ID,PersonName,DateEmis,Amount,Status,Impresa";
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

                ViewBag.clients = clients;
                ViewBag.conds = conds;
                ViewBag.sellers = sellers;
                ViewBag.transports = transports;
                ViewBag.currencies = currencies;
                ViewBag.formato = formato;

                return View();
            }
        }
    }
}