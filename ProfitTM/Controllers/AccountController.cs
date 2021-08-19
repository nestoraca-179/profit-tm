using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

namespace ProfitTM.Controllers
{
    public class AccountController : Controller
    {
        string DBProfitTM = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

        [HttpPost]
        public ActionResult Login(string username, string pass)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass))
            {
                using (SqlConnection conn = new SqlConnection(DBProfitTM))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("SELECT * FROM Users WHERE username = @Username AND pass = @Pass", conn))
                    {
                        comm.Parameters.AddWithValue("@Username", username);
                        comm.Parameters.AddWithValue("@Pass", pass);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FormsAuthentication.SetAuthCookie(username, true);
                                Session["user"] = reader["descrip"].ToString();

                                return RedirectToAction("SeleccionProducto", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", new { message = "Usuario o clave incorrectos" });
                            }
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { message = "Debes ingresar usuario y clave" });
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["user"] = null;
            Session["options"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}