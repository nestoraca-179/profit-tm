using ProfitTM.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using System;

namespace ProfitTM.Controllers
{
    public class HomeController : Controller
    {
        // Vista principal del login
        public ActionResult Index(string message = "")
        {
            ViewBag.user = Session["user"];
            ViewBag.home = Session["home"];
            ViewBag.Message = message;

            if (ViewBag.user != null && ViewBag.home != null)
            {
                return RedirectToAction(Session["home"].ToString());
            }
            else
            {
                ViewBag.user = new Users()
                {
                    Descrip = "null"
                };

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
        public ActionResult GuardarConexion(string name, string server, string db, string username_conn, string password_conn, string prod)
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
            ViewBag.prod = prod;
            ViewBag.user = Session["user"];
            ViewBag.modules = Session["modules"];

            Session["prod"] = prod;

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
                    ViewBag.connections = connections;
                    return View();
                }
            }
        }

        // Seleccion de sucursal
        [Authorize]
        public ActionResult SeleccionSucursal()
        {
            ViewBag.user = Session["user"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                Branch braManager = new Branch();
                ViewBag.branchs = braManager.GetAllBranchs();

                return View();
            }
        }
        
        // Seleccion de dashboard segun el aplicativo
        [Authorize]
        [HttpPost]
        public ActionResult SelectDashboard(string connect = "", bool connected = false)
        {
            ViewBag.user = Session["user"];
            string prod = Session["prod"].ToString();

            if (!connected)
            {
                Connections conn = new Connections();
                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    conn = db.Connections.First(c => c.ID.ToString() == connect);
                }

                string connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", conn.Server, conn.DB, conn.Username, conn.Password);
                SqlConnection connection = new SqlConnection(connectionString);

                Session["connect"] = connectionString;
                Session["DB"] = connection.Database;
                Session["name_conn"] = conn.Name;
            }
            else
            {
                Session["branch"] = connect;
            }

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
            }
            else
            {
                bool useBranchs = false;

                if (prod == "ADM")
                    useBranchs = new Branch().UseBranchs();

                if (useBranchs && Session["branch"] == null)
                {
                    return RedirectToAction("SeleccionSucursal");
                }
                else
                {
                    switch (prod)
                    {
                        case "ADM":
                            Session["home"] = "DashboardAdmin";
                            break;
                        case "CON":
                            Session["home"] = "DashboardCont";
                            break;
                        case "NOM":
                            Session["home"] = "DashboardNomi";
                            break;
                    }

                    return RedirectToAction(Session["home"].ToString());
                }
            }
        }

        // Dashboard admin
        [Authorize]
        public ActionResult DashboardAdmin()
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
                DateTime today = DateTime.Now;
                ViewBag.current_month = today.Month > 10 ? today.Month.ToString() : "0" + today.Month;
                ViewBag.current_year = today.Year;

                if (Session["modules"] == null)
                {
                    string userID = ((Users)ViewBag.user).ID.ToString();
                    Session["modules"] = Module.GetModulesByUser("ADM", userID);
                }

                string name_branch = "N/A";
                if (Session["branch"] != null)
                {
                    name_branch = new Branch().GetBranchByID(Session["branch"].ToString()).sucur_des;
                }

                Session["user_conn"] = ViewBag.user.Descrip;
                Session["data_conn"] = Session["name_conn"].ToString();
                Session["bran_conn"] = name_branch;

                ViewBag.user_conn = Session["user_conn"].ToString();
                ViewBag.data_conn = Session["data_conn"].ToString();
                ViewBag.bran_conn = Session["bran_conn"].ToString();

                ViewBag.modules = Session["modules"];

                return View();
            }
        }

        // Dashboard cont
        [Authorize]
        public ActionResult DashboardCont()
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
                if (Session["modules"] == null)
                {
                    string userID = ((Users)ViewBag.user).ID.ToString();
                    Session["modules"] = Module.GetModulesByUser("CON", userID);
                }

                Session["user_conn"] = ViewBag.user.Descrip;
                Session["data_conn"] = Session["name_conn"].ToString();

                ViewBag.user_conn = Session["user_conn"].ToString();
                ViewBag.data_conn = Session["data_conn"].ToString();

                ViewBag.modules = Session["modules"];

                return View();
            }
        }

        // Dashboard nomi
        [Authorize]
        public ActionResult DashboardNomi()
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
                if (Session["modules"] == null)
                {
                    string userID = ((Users)ViewBag.user).ID.ToString();
                    Session["modules"] = Module.GetModulesByUser("NOM", userID);
                }

                Session["user_conn"] = ViewBag.user.Descrip;
                Session["data_conn"] = Session["name_conn"].ToString();

                ViewBag.user_conn = Session["user_conn"].ToString();
                ViewBag.data_conn = Session["data_conn"].ToString();

                ViewBag.modules = Session["modules"];

                return View();
            }
        }
    }
}