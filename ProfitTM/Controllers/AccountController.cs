using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using System.Data.Entity;
using ProfitTM.Models;
using System;

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
                // string decryptedText = SecurityController.Decrypt("SvxYb3wGpTYQAvMRdCSAXQ==");

                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    Users user = db.Users.AsNoTracking().SingleOrDefault(u => u.Username == username && u.Password == encryptedPass);
                    
                    if (user == null)
                    {
                        return RedirectToAction("Index", "Home", new { message = "Usuario o clave incorrectos" });
                    }
                    else
                    {
                        if (!user.Enabled)
                            return RedirectToAction("Index", "Home", new { message = "Usuario inactivo" });

                        FormsAuthentication.SetAuthCookie(username, false);
                        Session["USER"] = user;

                        if (DateTime.Compare(user.NextChange, DateTime.Now) < 0)
                            return RedirectToAction("CambiarClave", "Home");

                        return RedirectToAction("SeleccionProducto", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { message = "Debes ingresar usuario y clave" });
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string username, string old_pass, string new_pass)
        {
            if (!string.IsNullOrEmpty(old_pass) && !string.IsNullOrEmpty(new_pass))
            {
                string encryptedOldPass = SecurityController.Encrypt(old_pass);
                string encryptedNewPass = SecurityController.Encrypt(new_pass);

                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    Users user = db.Users.AsNoTracking().SingleOrDefault(u => u.Username == username && u.Password == encryptedOldPass);

                    if (user == null)
                    {
                        return RedirectToAction("CambiarClave", "Home", new { message = "Usuario o clave incorrectos" });
                    }
                    else
                    {
                        if (old_pass.Trim() == new_pass.Trim())
                            return RedirectToAction("CambiarClave", "Home", new { message = "Debes ingresar una clave diferente a la anterior" });

                        user.Password = encryptedNewPass;
                        user.NextChange = DateTime.Now.AddMonths(3);
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("SeleccionProducto", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("CambiarClave", "Home", new { message = "Debes ingresar la clave anterior y la clave nueva" });
            }
        }

        public ActionResult Logout(string msg = "")
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
            }

            if (msg != "")
                return RedirectToAction("Index", "Home", new { message = msg });

            return RedirectToAction("Index", "Home");
        }
    }
}