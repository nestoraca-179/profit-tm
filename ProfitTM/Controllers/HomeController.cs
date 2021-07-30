using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Web;
using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Controllers
{
    public class HomeController : Controller
    {
        // Vista principal del login
        public ActionResult Index(string message = "")
        {
            ViewBag.user = Session["user"];
            ViewBag.Message = message;

            if (ViewBag.user != null)
            {
                return RedirectToAction(Session["home"].ToString());
            }
            else
            {
                return View();
            }
        }

        // Seleccion de aplicativo
        [Authorize]
        public ActionResult SeleccionProducto()
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                return View();
            }
        }

        // Seleccion de empresa
        [Authorize]
        public ActionResult SeleccionEmpresa(string prod)
        {
            ViewBag.prod = prod;
            ViewBag.user = Session["user"];
            ViewBag.connections = ConfigurationManager.ConnectionStrings;

            Session["Prod"] = prod;

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                return View();
            }
        }

        // Seleccion de dashboard segun el aplicativo
        [Authorize]
        [HttpPost]
        public ActionResult SelectDashboard(string connect = "")
        {
            string prod = Session["Prod"].ToString();

            Session["connect"] = connect;
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                switch (prod)
                {
                    case "Admin":
                        Session["home"] = "DashboardAdmin";
                        return RedirectToAction("DashboardAdmin");
                    case "Cont":
                        Session["home"] = "DashboardCont";
                        return RedirectToAction("DashboardCont");
                    case "Nomi":
                        break;
                }
            }

            return null;
        }

        // Dashboard admin
        [Authorize]
        public ActionResult DashboardAdmin()
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                SQLController sqlController = new SQLController();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseOPT = new SQLController("MainConnection").getOptions("Admin");
                ProfitTMResponse responseMSP = sqlController.getMostSelledProducts(5);
                ProfitTMResponse responseMPP = sqlController.getMostPurchasedProducts(5);
                ProfitTMResponse responseMAC = sqlController.getMostActiveClients(5);
                ProfitTMResponse responseMMC = sqlController.getMostMorousClients(5);
                ProfitTMResponse responseMAS = sqlController.getMostActiveSuppliers(5);
                ProfitTMResponse responseMMS = sqlController.getMostMorousSuppliers(5);

                responses.Add(responseOPT);
                responses.Add(responseMSP);
                responses.Add(responseMPP);
                responses.Add(responseMAC);
                responses.Add(responseMMC);
                responses.Add(responseMAS);
                responses.Add(responseMMS);

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
                    Session["options"] = responseOPT.Result;

                    ViewBag.mostSelledProds = responseMSP.Result;
                    ViewBag.mostPurchasedProds = responseMPP.Result;
                    ViewBag.mostActiveClients = responseMAC.Result;
                    ViewBag.mostMorousClients = responseMMC.Result;
                    ViewBag.mostActiveSuppliers = responseMAS.Result;
                    ViewBag.mostMorousSuppliers = responseMMS.Result;
                    ViewBag.options = Session["options"];

                    return View();
                }
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", new { message = msg });
                }
            }
        }

        // Dashboard cont
        [Authorize]
        public ActionResult DashboardCont()
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                SQLController sqlController = new SQLController();

                bool error = false;
                string msg = "";

                ProfitTMResponse responseOPT = new SQLController("MainConnection").getOptions("Cont");

                responses.Add(responseOPT);

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
                    Session["options"] = responseOPT.Result;

                    ViewBag.options = Session["options"];

                    return View();
                }
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", new { message = msg });
                }
            }
        }

        // Opciones Admin
        [Authorize]
        public ActionResult Inventario()
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                SQLController sqlController = new SQLController();

                ProfitTMResponse responseMSP = sqlController.getMostSelledProducts(10);
                ProfitTMResponse responseMPP = sqlController.getMostPurchasedProducts(10);

                bool error = false;
                string msg = "";

                responses.Add(responseMSP);
                responses.Add(responseMPP);

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
                    ViewBag.mostSelledProds = responseMSP.Result;
                    ViewBag.mostPurchasedProds = responseMPP.Result;
                    ViewBag.options = Session["options"];

                    return View();
                }
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", new { message = msg });
                }
            }
        }

        [Authorize]
        public ActionResult Ventas()
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                SQLController sqlController = new SQLController();

                ProfitTMResponse responseMAC = sqlController.getMostActiveClients(10);
                ProfitTMResponse responseMMC = sqlController.getMostMorousClients(10);

                bool error = false;
                string msg = "";

                responses.Add(responseMAC);
                responses.Add(responseMMC);

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
                    ViewBag.mostActiveClients = responseMAC.Result;
                    ViewBag.mostMorousClients = responseMMC.Result;
                    ViewBag.options = Session["options"];

                    return View();
                }
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", new { message = msg });
                }
            }
        }

        [Authorize]
        public ActionResult Compras()
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                SQLController sqlController = new SQLController();

                ProfitTMResponse responseMAS = sqlController.getMostActiveSuppliers(10);
                ProfitTMResponse responseMMS = sqlController.getMostMorousSuppliers(10);

                bool error = false;
                string msg = "";

                responses.Add(responseMAS);
                responses.Add(responseMMS);

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
                    ViewBag.mostActiveSuppliers = responseMAS.Result;
                    ViewBag.mostMorousSuppliers = responseMMS.Result;
                    ViewBag.options = Session["options"];

                    return View();
                }
                else
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", new { message = msg });
                }
            }
        }
    }
}