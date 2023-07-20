using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Collections.Generic;
using ProfitTM.Models;

namespace ProfitTM.Controllers
{
    public class HomeController : Controller
    {
        // Vista principal del login
        public ActionResult Index(string message = "")
        {
            if (Request.IsAuthenticated && Session["USER"] != null)
            {
                if (Session["PROD"] == null)
                    return RedirectToAction("SeleccionProducto");

                if (Session["CONNECT"] == null)
                    return RedirectToAction("SeleccionEmpresa");

                if (Session["BRANCH"] == null && Session["PROD"].ToString() == "ADM" && new Branch().UseBranchs())
                    return RedirectToAction("SeleccionSucursal");

                return RedirectToAction(Session["HOME"].ToString());
            }
            else
            {
                ViewBag.Message = message;
                ViewBag.user = new Users() { Descrip = "null" };

                return View();
            }
        }

        // Cambiar clave
        public ActionResult CambiarClave(string message = "")
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else 
            {
                ViewBag.user = Session["USER"];
                ViewBag.Message = message;

                return View();
            }
        }
        
        // Seleccion de aplicativo
        [Authorize]
        public ActionResult SeleccionProducto()
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                ViewBag.user = Session["USER"];
                return View();
            }
        }

        // Agregar Conexion
        [Authorize]
        public ActionResult AgregarConexion(string product, string message = "")
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                ViewBag.prod = product;
                ViewBag.message = message;
                ViewBag.user = Session["USER"];

                return View();
            }
        }

        // Guardar Conexion
        [Authorize]
        [HttpPost]
        public ActionResult GuardarConexion(string name, string server, string db, string username_conn, string password_conn, string prod)
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                bool connected;
                string connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", server, db, username_conn, password_conn), msg = "";

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
                    Connections conn = new Connections() {
                        Name = name,
                        Server = server,
                        DB = db,
                        Username = username_conn,
                        Password = password_conn,
                        Type = prod
                    };

                    ProfitTMResponse result = Connection.Add(conn);

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
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (Session["MODULES"] != null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión nuevamente" });
            }
            else if (prod == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir un aplicativo" });
            }
            else
            {
                List<Connections> connections = Connection.GetConnectionsByType(prod);
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
                    Session["PROD"] = prod;
                    ViewBag.user = Session["USER"];
                    ViewBag.connections = connections;

                    return View();
                }
            }
        }

        // Seleccion de sucursal
        [Authorize]
        public ActionResult SeleccionSucursal()
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                ViewBag.user = Session["USER"];
                ViewBag.branchs = new Branch().GetAllBranchs();

                return View();
            }
        }
        
        // Seleccion de dashboard segun el aplicativo
        [Authorize]
        [HttpPost]
        public ActionResult SelectDashboard(string connect = "", bool connected = false)
        {
            if (!connected)
            {
                Connections conn = Connection.GetConnByID(connect);
                string connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", conn.Server, conn.DB, conn.Username, conn.Password);
                SqlConnection connection = new SqlConnection(connectionString);

                Session["CONNECT"] = connectionString;
                Session["DB"] = connection.Database;
                Session["NAME_CONN"] = conn.Name;
                Session["ID_CONN"] = conn.ID;
            }
            else
            {
                Session["BRANCH"] = connect;
            }

            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                bool useBranchs = false;
                string prod = Session["PROD"].ToString();

                if (prod == "ADM")
                    useBranchs = new Branch().UseBranchs();

                if (useBranchs && Session["BRANCH"] == null)
                    return RedirectToAction("SeleccionSucursal");

                switch (prod)
                {
                    case "ADM":
                        Session["HOME"] = "DashboardAdmin";
                        break;
                    case "CON":
                        Session["HOME"] = "DashboardCont";
                        break;
                    case "NOM":
                        Session["HOME"] = "DashboardNomi";
                        break;
                }

                return RedirectToAction(Session["HOME"].ToString());
            }
        }

        // Dashboard admin
        [Authorize]
        public ActionResult DashboardAdmin()
        {
            // System.Web.HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];//.ASPXAUTH
            // FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Logout", "Account", new { message = "Debes iniciar sesión" });
            }
            else if (Session["CONNECT"] == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                // DateTime today = DateTime.Now;
                // ViewBag.current_month = today.Month > 10 ? today.Month.ToString() : "0" + today.Month;
                // ViewBag.current_year = today.Year;

                if (Session["MODULES"] == null)
                {
                    string userID = ((Users)Session["USER"]).ID.ToString();
                    Session["MODULES"] = Module.GetModulesByUser("ADM", userID);
                }

                string name_branch = "N/A";
                if (Session["BRANCH"] != null)
                    name_branch = new Branch().GetBranchByID(Session["BRANCH"].ToString()).sucur_des;

                Session["DATA_CONN"] = Session["NAME_CONN"].ToString();
                Session["BRAN_CONN"] = name_branch;
                ViewBag.user = Session["USER"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.product = "Administrativo";
                ViewBag.modules = Session["MODULES"];
                ViewBag.db = Session["DB"];

                return View();
            }
        }

        // Dashboard cont
        [Authorize]
        public ActionResult DashboardCont()
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (Session["CONNECT"] == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                if (Session["MODULES"] == null)
                {
                    string userID = ((Users)Session["USER"]).ID.ToString();
                    Session["MODULES"] = Module.GetModulesByUser("CON", userID);
                }

                Session["DATA_CONN"] = Session["NAME_CONN"].ToString();
                ViewBag.user = Session["USER"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.product = "Contabilidad";
                ViewBag.modules = Session["MODULES"];

                return View();
            }
        }

        // Dashboard nomi
        [Authorize]
        public ActionResult DashboardNomi()
        {
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else if (Session["CONNECT"] == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                if (Session["MODULES"] == null)
                {
                    string userID = ((Users)Session["USER"]).ID.ToString();
                    Session["MODULES"] = Module.GetModulesByUser("NOM", userID);
                }

                Session["DATA_CONN"] = Session["NAME_CONN"].ToString();
                ViewBag.user = Session["USER"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.product = "Nómina";
                ViewBag.modules = Session["MODULES"];

                return View();
            }
        }
    }
}