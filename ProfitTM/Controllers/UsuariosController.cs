using ProfitTM.Models;
using System.Web.Mvc;
using MyUser = ProfitTM.Models.User;

namespace ProfitTM.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.connect = Session["CONNECT"];
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                switch (Session["PROD"].ToString())
                {
                    case "ADM":
                        ViewBag.product = "Administrativo";
                        break;
                    case "CON":
                        ViewBag.product = "Contabilidad";
                        break;
                    case "NOM":
                        ViewBag.product = "Nómina";
                        break;
                }

                ViewBag.user = Session["USER"];
                ViewBag.modules = Session["MODULES"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"]?.ToString();
                ViewBag.users = MyUser.GetAllUsers(false);

                return View();
            }
        }

        public ActionResult Agregar()
        {
            ViewBag.connect = Session["CONNECT"];
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                switch (Session["PROD"].ToString())
                {
                    case "ADM":
                        ViewBag.product = "Administrativo";
                        break;
                    case "CON":
                        ViewBag.product = "Contabilidad";
                        break;
                    case "NOM":
                        ViewBag.product = "Nómina";
                        break;
                }

                ViewBag.user = Session["USER"];
                ViewBag.modules = Session["MODULES"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"]?.ToString();
                ViewBag.all_mods = Module.GetAllModules();
                ViewBag.sups = MyUser.GetAllUsers(true);

                return View();
            }
        }

        public ActionResult Editar(string id)
        {
            ViewBag.connect = Session["CONNECT"];
            if (!Request.IsAuthenticated)
            {
                // FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                switch (Session["PROD"].ToString())
                {
                    case "ADM":
                        ViewBag.product = "Administrativo";
                        break;
                    case "CON":
                        ViewBag.product = "Contabilidad";
                        break;
                    case "NOM":
                        ViewBag.product = "Nómina";
                        break;
                }

                ViewBag.user = Session["USER"];
                ViewBag.modules = Session["MODULES"];
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"]?.ToString();
                ViewBag.user_edit = MyUser.GetUserByID(id);
                ViewBag.all_mods = Module.GetAllModules();

                return View();
            }
        }
    }
}