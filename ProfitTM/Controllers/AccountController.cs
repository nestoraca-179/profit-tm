using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using ProfitTM.Models;

namespace ProfitTM.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public ActionResult Login(string username, string pass)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pass))
            {
                string encryptedPass = SecurityController.Encrypt(pass);
                //string decryptedText = SecurityController.Decrypt("YLXp9G9P1Gt0MM7BbVo4BA==");

                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    Users user = db.Users.SingleOrDefault(u => u.Username == username && u.Password == encryptedPass);
                    
                    if (user != null)
                    {
                        if (user.Enabled)
                        {
                            FormsAuthentication.SetAuthCookie(username, true);
                            Session["user"] = user;

                            return RedirectToAction("SeleccionProducto", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { message = "Usuario inactivo" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { message = "Usuario o clave incorrectos" });
                    }
                }

                #region CODIGO ANTERIOR
                /*using (SqlConnection conn = new SqlConnection(DBProfitTM))
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
                                if (bool.Parse(reader["Enabled"].ToString()))
                                {
                                    FormsAuthentication.SetAuthCookie(username, true);

                                    User user = new User()
                                    {
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
                                    return RedirectToAction("Index", "Home", new { message = "Usuario inactivo" });
                                }
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", new { message = "Usuario o clave incorrectos" });
                            }
                        }
                    }
                }*/
                #endregion
            }
            else
            {
                return RedirectToAction("Index", "Home", new { message = "Debes ingresar usuario y clave" });
            }
        }

        public ActionResult Logout(string msg = "")
        {
            FormsAuthentication.SignOut();
            Session.Clear();

            if (msg != "")
            {
                return RedirectToAction("Index", "Home", new { message = msg });
            }

            return RedirectToAction("Index", "Home");
        }
    }
}