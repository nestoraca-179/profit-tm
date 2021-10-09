using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;
using ProfitTM.Models;

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
                string encryptedPass = SecurityController.Encrypt(pass);
                //string decryptedText = SecurityController.Decrypt(encryptedPass);

                using (SqlConnection conn = new SqlConnection(DBProfitTM))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("SELECT * FROM Users WHERE Username = @Username AND Password = @Pass", conn))
                    {
                        comm.Parameters.AddWithValue("@Username", username);
                        comm.Parameters.AddWithValue("@Pass", encryptedPass);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FormsAuthentication.SetAuthCookie(username, true);
                                
                                User user = new User() {
                                    ID = reader["ID"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Descrip = reader["Descrip"].ToString(),
                                    IsAdm = bool.Parse(reader["IsAdm"].ToString()),
                                    IsCon = bool.Parse(reader["IsCon"].ToString()),
                                    IsNom = bool.Parse(reader["IsNom"].ToString())
                                };

                                Session["user"] = user;

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

        public ActionResult Logout(string msg = "")
        {
            FormsAuthentication.SignOut();
            Session["user"] = null;
            Session["options"] = null;

            if (msg != "")
            {
                return RedirectToAction("Index", "Home", new { message = msg });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}