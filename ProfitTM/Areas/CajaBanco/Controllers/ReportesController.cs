using ProfitTM.Controllers;
using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.CajaBanco.Controllers
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
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                List<string> parameters = new List<string>(), qParam = new List<string>();
                SQLController sqlController = new SQLController();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseAB = sqlController.getBanks();
                ProfitTMResponse responseAA = sqlController.getBankAccount();

                responses.Add(responseAB);
                responses.Add(responseAA);

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
                    ViewBag.assistCods = responseAB.Result;
                    ViewBag.assistAccs = responseAA.Result;

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
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", "Home", new { area = "", message = msg });
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