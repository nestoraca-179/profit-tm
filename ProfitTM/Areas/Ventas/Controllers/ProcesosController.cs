using ProfitTM.Controllers;
using ProfitTM.Models;
using ProfitTM.Enum;
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
                //InvoiceManager invoiceManager = new InvoiceManager();
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
                    List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

                    NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                    formato.CurrencyGroupSeparator = ".";
                    formato.NumberDecimalSeparator = ",";

                    ViewBag.assistClients = responseAC.Result;

                    if (function == EnumLoadFunction.INVOICEV)
                    {
                        InvoiceManager invoiceManager = new InvoiceManager();
                        result = invoiceManager.getAllInvoices("V");

                        if (result.Status == "OK")
                        {
                            List<Invoice> invoices = (List<Invoice>)result.Result;

                            foreach (Invoice invoice in invoices)
                            {
                                Dictionary<string, string> item = new Dictionary<string, string>();

                                item.Add("ID", invoice.ID);
                                item.Add("cli_des", invoice.Descrip);
                                item.Add("fec_emis", invoice.Date.ToString());
                                item.Add("total_neto", invoice.Amount.ToString("N", formato));
                                item.Add("details", "True");
                                item.Add("type", "V");

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

                                results.Add(item);
                            }

                            ViewBag.resultsTable = results;
                            ViewBag.titleR = "Factura";
                            ViewBag.headers = "Codigo,Cliente,Fecha,Total,Estado,Impresa";
                            ViewBag.cols = "ID,cli_des,fec_emis,total_neto,status,impresa";
                        }
                        else
                        {
                            ViewBag.results = "";
                        }
                    }
                    else if (function == EnumLoadFunction.INVOICEPV)
                    {
                        InvoiceManager invoiceManager = new InvoiceManager();
                        result = invoiceManager.getAllInvoices("PV");

                        if (result.Status == "OK")
                        {
                            List<Invoice> invoices = (List<Invoice>)result.Result;

                            foreach (Invoice invoice in invoices)
                            {
                                Dictionary<string, string> item = new Dictionary<string, string>();

                                item.Add("ID", invoice.ID);
                                item.Add("cli_des", invoice.Descrip);
                                item.Add("fec_emis", invoice.Date.ToString());
                                item.Add("total_neto", invoice.Amount.ToString("N", formato));
                                item.Add("details", "True");
                                item.Add("type", "PV");

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

                                results.Add(item);
                            }

                            ViewBag.resultsTable = results;
                            ViewBag.titleR = "Plantilla";
                            ViewBag.headers = "Codigo,Cliente,Fecha,Total,Estado,Impresa";
                            ViewBag.cols = "ID,cli_des,fec_emis,total_neto,status,impresa";
                        }
                        else
                        {
                            ViewBag.results = "";
                        }
                    }
                    else
                    {
                        ViewBag.results = "";
                    }

                    #region
                    /*switch (option)
                    {
                        case "0":

                            result = invoiceManager.getAllInvoices('V');

                            if (result.Status == "OK")
                            {
                                List<Invoice> invoices = (List<Invoice>)result.Result;

                                foreach (Invoice invoice in invoices)
                                {
                                    Dictionary<string, string> item = new Dictionary<string, string>();

                                    item.Add("doc_num", invoice.ID);
                                    item.Add("cli_des", invoice.Descrip);
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
                                    {
                                        item.Add("impresa", "SI");
                                    }
                                    else
                                    {
                                        item.Add("impresa", "NO");
                                    }

                                    results.Add(item);
                                }

                                ViewBag.resultsTable = results;
                                ViewBag.titleR = "Factura";
                                ViewBag.headers = "Codigo,Cliente,Fecha,Total,Estado,Impresa";
                                ViewBag.cols = "doc_num,cli_des,fec_emis,total_neto,status,impresa";
                            }
                            else
                            {
                                ViewBag.results = "";
                            }

                            break;
                        default:
                            ViewBag.results = "";
                            break;
                    }*/
                    #endregion

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