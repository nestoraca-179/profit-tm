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
                // string decryptedText = SecurityController.Decrypt("YLXp9G9P1Gt0MM7BbVo4BA==");

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

                        FormsAuthentication.SetAuthCookie(username, true);
                        Session["USER"] = user;

                        return RedirectToAction("SeleccionProducto", "Home");
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
            Session.Clear();

            if (msg != "")
                return RedirectToAction("Index", "Home", new { message = msg });

            return RedirectToAction("Index", "Home");
        }
    }
}