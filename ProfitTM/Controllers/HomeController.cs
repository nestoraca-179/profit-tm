using ProfitTM.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;

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

        // Agregar Conexion
        [Authorize]
        public ActionResult AgregarConexion(string product, string message = "")
        {
            ViewBag.user = Session["user"];
            ViewBag.prod = product;
            ViewBag.message = message;

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

        // Guardar Conexion
        [Authorize]
        [HttpPost]
        public ActionResult GuardarConexion(string name, string server, string db, string username, string password, string prod)
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                bool connected;
                string msg = "";
                string connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", server, db, username, password);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand("select * from par_emp", conn))
                        {
                            comm.ExecuteNonQuery();
                        }

                        connected = true;
                        msg = "Conexión exitosa";
                    }
                    catch (SqlException ex)
                    {
                        connected = false;
                        msg = ex.Message;
                    }
                }

                if (connected)
                {
                    ProfitTMResponse result = Connection.SaveConn(name, server, db, username, password, prod);

                    if (result.Status == "OK")
                    {
                        string type = prod;
                        return RedirectToAction("SeleccionEmpresa", new { prod = type });
                    }
                    else
                    {
                        msg = "Ha ocurrido un error al guardar la conexión => " + result.Message;
                        return RedirectToAction("AgregarConexion", new { message = msg });
                    }
                }
                else
                {
                    return RedirectToAction("AgregarConexion", new { message = "Conexión fallida => " + msg });
                }
            }
        }

        // Seleccion de empresa
        [Authorize]
        public ActionResult SeleccionEmpresa(string prod)
        {
            ViewBag.prod = prod;
            ViewBag.user = Session["user"];
            ViewBag.modules = Session["modules"];
            //ViewBag.connections = ConfigurationManager.ConnectionStrings;

            Session["Prod"] = prod;

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.modules != null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión nuevamente" });
            }
            else if (prod == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                List<Connection> connections = Connection.GetConnections(prod);

                if (connections == null)
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", new { message = "Ha ocurrido un error recuperando las conexiones" });
                }
                else if (connections.Count == 0)
                {
                    return RedirectToAction("AgregarConexion", new { product = prod });
                }
                else
                {
                    ViewBag.connections = connections;
                    return View();
                }
            }
        }

        // Seleccion de dashboard segun el aplicativo
        [Authorize]
        [HttpPost]
        public ActionResult SelectDashboard(string connect = "")
        {
            ViewBag.user = Session["user"];
            string prod = Session["Prod"].ToString();

            Connection conn = Connection.GetConnections(prod).Find(c => c.ID == connect);
            string connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", conn.Server, conn.DB, conn.Username, conn.Password);

            Session["connect"] = connectionString;

            //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connect].ConnectionString);
            SqlConnection connection = new SqlConnection(connectionString);
            Session["DB"] = connection.Database;

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
                        break;
                    case "Cont":
                        Session["home"] = "DashboardCont";
                        break;
                    case "Nomi":
                        Session["home"] = "DashboardNomi";
                        break;
                }

                return RedirectToAction(Session["home"].ToString());
            }
        }

        // Dashboard admin
        [Authorize]
        public ActionResult DashboardAdmin()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            ViewBag.session = Session;

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                if (Session["modules"] == null)
                {
                    bool error = false;
                    string msg = "", userID = ((User)ViewBag.user).ID;

                    List<Module> modules = Module.GetModules("ADM", userID);
                    List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                    SQLController sqlController = new SQLController();

                    ProfitTMResponse responseMSP = sqlController.getMostSelledProducts(5);
                    ProfitTMResponse responseMPP = sqlController.getMostPurchasedProducts(5);
                    ProfitTMResponse responseMAC = sqlController.getMostActiveClients(5);
                    ProfitTMResponse responseMMC = sqlController.getMostMorousClients(5);
                    ProfitTMResponse responseMAS = sqlController.getMostActiveSuppliers(5);
                    ProfitTMResponse responseMMS = sqlController.getMostMorousSuppliers(5);

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
                        Session["MSP"] = responseMSP.Result;
                        Session["MPP"] = responseMPP.Result;
                        Session["MAC"] = responseMAC.Result;
                        Session["MMC"] = responseMMC.Result;
                        Session["MAS"] = responseMAS.Result;
                        Session["MMS"] = responseMMS.Result;
                        Session["modules"] = modules;
                    }
                    else
                    {
                        return RedirectToAction("Logout", "Account", new { msg = msg });
                    }
                }

                ViewBag.mostSelledProds = Session["MSP"];
                ViewBag.mostPurchasedProds = Session["MPP"];
                ViewBag.mostActiveClients = Session["MAC"];
                ViewBag.mostMorousClients = Session["MMC"];
                ViewBag.mostActiveSuppliers = Session["MAS"];
                ViewBag.mostMorousSuppliers = Session["MMS"];
                ViewBag.modules = Session["modules"];

                NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                formato.CurrencyGroupSeparator = ".";
                formato.NumberDecimalSeparator = ",";
                ViewBag.formato = formato;

                return View();
            }
        }

        // Dashboard cont
        [Authorize]
        public ActionResult DashboardCont()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            string userID = ((User)ViewBag.user).ID;

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                if (Session["modules"] == null)
                {
                    List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                    SQLController sqlController = new SQLController();

                    bool error = false;
                    string msg = "";

                    List<Module> modules = Module.GetModules("CON", userID);
                    ProfitTMResponse responseLQT = sqlController.getLiqCapTest(1);
                    ProfitTMResponse responseCPT = sqlController.getLiqCapTest(2);

                    responses.Add(responseLQT);
                    responses.Add(responseCPT);

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
                        Session["LQT"] = responseLQT.Result;
                        Session["CPT"] = responseCPT.Result;
                        Session["modules"] = modules;
                    }
                    else
                    {
                        return RedirectToAction("Logout", "Account", new { msg = msg });
                    }
                }

                ViewBag.testLiq = Session["LQT"];
                ViewBag.testCap = Session["CPT"];
                ViewBag.modules = Session["modules"];

                NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                formato.CurrencyGroupSeparator = ".";
                formato.NumberDecimalSeparator = ",";
                ViewBag.formato = formato;

                return View();
            }
        }

        // Dashboard nomi
        [Authorize]
        public ActionResult DashboardNomi()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            string userID = ((User)ViewBag.user).ID;

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                if (Session["modules"] == null)
                {
                    List<ProfitTMResponse> responses = new List<ProfitTMResponse>();
                    SQLController sqlController = new SQLController();

                    bool error = false;
                    string msg = "";

                    List<Module> modules = Module.GetModules("NOM", userID);

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
                        Session["modules"] = modules;
                    }
                    else
                    {
                        return RedirectToAction("Logout", "Account", new { msg = msg });
                    }
                }

                ViewBag.modules = Session["modules"];

                NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                formato.CurrencyGroupSeparator = ".";
                formato.NumberDecimalSeparator = ",";
                ViewBag.formato = formato;

                return View();
            }
        }

        // Opciones Admin
        [Authorize]
        public ActionResult Inventario()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
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
                    ViewBag.modules = Session["modules"];

                    NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                    formato.CurrencyGroupSeparator = ".";
                    formato.NumberDecimalSeparator = ",";
                    ViewBag.formato = formato;

                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Account", new { msg = msg });
                }
            }
        }

        [Authorize]
        public ActionResult Ventas()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
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
                    ViewBag.modules = Session["modules"];

                    NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                    formato.CurrencyGroupSeparator = ".";
                    formato.NumberDecimalSeparator = ",";
                    ViewBag.formato = formato;

                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Account", new { msg = msg });
                }
            }
        }

        [Authorize]
        public ActionResult Compras()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
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
                    ViewBag.modules = Session["modules"];

                    NumberFormatInfo formato = new CultureInfo("es-ES").NumberFormat;
                    formato.CurrencyGroupSeparator = ".";
                    formato.NumberDecimalSeparator = ",";
                    ViewBag.formato = formato;

                    return View();
                }
                else
                {
                    return RedirectToAction("Logout", "Account", new { msg = msg });
                }
            }
        }
    }
}