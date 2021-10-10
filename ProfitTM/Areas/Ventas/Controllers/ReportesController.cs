using ProfitTM.Controllers;
using ProfitTM.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class ReportesController : Controller
    {
        public ActionResult Index(string name = "", string proc = "", string cols = "", string fields = "", string queryParams = "", string[] paramsSent = null, string format = "")
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

                List<Product> products = Product.GetAllProducts(connect);
                List<Client> clients = Client.GetAllClients(connect);

                SQLController sqlController = new SQLController();
                List<string> parameters = new List<string>(), qParam = new List<string>();

                ViewBag.assistProds = products;
                ViewBag.assistClients = clients;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(proc) && !string.IsNullOrEmpty(cols) && !string.IsNullOrEmpty(fields))
                {
                    List<string> fieldsToShow = new List<string>(), colsToShow = new List<string>();

                    if (paramsSent != null)
                    {
                        int ind = 0;
                        foreach (string par in paramsSent)
                        {
                            if (par != "")
                            {
                                parameters.Add(par);
                                qParam.Add(queryParams.Split(',')[ind]);
                            }

                            ind++;
                        }
                    }

                    foreach (string str in cols.Split(','))
                    {
                        if (!str.Contains("$"))
                        {
                            if (str.Contains("#"))
                                colsToShow.Add(str.Replace("#", ""));
                            else
                                colsToShow.Add(str);
                        }
                    }
                    foreach (string str in fields.Split(','))
                    {
                        fieldsToShow.Add(str);
                    }

                    ProfitTMResponse result = sqlController.getResultsReports(proc, cols, parameters, qParam);

                    if (result.Status == "OK")
                    {
                        ViewBag.results = result.Result;

                        ViewBag.name = name;
                        ViewBag.cols = colsToShow;
                        ViewBag.fields = fieldsToShow;

                        if (ViewBag.results.Count > 0)
                        {
                            ViewBag.format = format;
                        }
                    }
                    else
                    {
                        ViewBag.message = result.Message;
                        ViewBag.name = "";
                        ViewBag.results = null;
                    }
                }

                return View();
            }
        }

        public ActionResult Reporte(string name)
        {
            ViewBag.user = Session["user"];
            ViewBag.options = Session["options"];

            ViewBag.report = name;

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