using ProfitTM.Controllers;
using ProfitTM.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Inventario.Controllers
{
    [Authorize]
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
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                List<string> parameters = new List<string>(), qParam = new List<string>();
                SQLController sqlController = new SQLController();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseAP = sqlController.getProds();
                ProfitTMResponse responseAR = sqlController.getPrices();
                ProfitTMResponse responseAS = sqlController.getStorages();

                responses.Add(responseAP);
                responses.Add(responseAR);
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
                    ViewBag.assistProds = responseAP.Result;
                    ViewBag.assistPrices = responseAR.Result;
                    ViewBag.assistStorages = responseAS.Result;

                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(proc) && !string.IsNullOrEmpty(fields) && !string.IsNullOrEmpty(cols))
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
                else
                {
                    return RedirectToAction("Logout", "Account", new { area = "", msg = msg });
                }
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