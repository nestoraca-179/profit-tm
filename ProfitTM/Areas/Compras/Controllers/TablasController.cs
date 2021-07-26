using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class TablasController : Controller
    {
        public ActionResult Index(string option = "", string[] paramsProvAdd = null, string[] paramsProvEdit = null, string ID = "")
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
                SQLController sqlController = new SQLController();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseAT = sqlController.getTypes();
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
                    ViewBag.results = option;

                    ProfitTMResponse result;

                    if (paramsProvAdd != null)
                    {
                        Supplier supplier = new Supplier()
                        {
                            ID = paramsProvAdd[0],
                            Name = paramsProvAdd[1],
                            Type = paramsProvAdd[2],
                            RIF = paramsProvAdd[3],
                            NIT = paramsProvAdd[4],
                            Zone = paramsProvAdd[5],
                            Account = paramsProvAdd[6],
                            Country = paramsProvAdd[7],
                            Segment = paramsProvAdd[8],
                            Email = paramsProvAdd[9],
                            Phone = paramsProvAdd[10],
                            Address = paramsProvAdd[11]
                        };

                        result = sqlController.addSupplier(supplier);

                        if (result.Status == "OK")
                        {
                            ViewBag.message = "Proveedor agregado con exito, " + result.Result + " filas afectadas";
                            ViewBag.classAlert = "alert alert-success";
                        }
                        else
                        {
                            ViewBag.message = result.Message;
                            ViewBag.classAlert = "alert alert-danger";
                        }
                    }
                    else if (paramsProvEdit != null)
                    {
                        Supplier supplier = new Supplier()
                        {
                            ID = paramsProvEdit[0].Trim(),
                            Name = paramsProvEdit[1].Trim(),
                            RIF = paramsProvEdit[2].Trim(),
                            Email = paramsProvEdit[3].Trim(),
                            Phone = paramsProvEdit[4].Trim(),
                            Address = paramsProvEdit[5].Trim()
                        };

                        result = sqlController.editSupplier(supplier);

                        if (result.Status == "OK")
                        {
                            ViewBag.message = "Proveedor actualizado con exito, " + result.Result + " filas afectadas";
                            ViewBag.classAlert = "alert alert-success";
                        }
                        else
                        {
                            ViewBag.message = result.Message;
                            ViewBag.classAlert = "alert alert-danger";
                        }
                    }
                    else if (!string.IsNullOrEmpty(ID))
                    {
                        result = sqlController.deleteSupplier(ID);

                        if (result.Status == "OK")
                        {
                            ViewBag.message = "Proveedor eliminado con exito, " + result.Result + " filas afectadas";
                            ViewBag.classAlert = "alert alert-success";
                        }
                        else
                        {
                            ViewBag.message = result.Message;
                            ViewBag.classAlert = "alert alert-danger";
                        }
                    }
                    else
                    {
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
    }
}