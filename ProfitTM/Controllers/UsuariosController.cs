using ProfitTM.Models;
using System.Web.Mvc;
using System.Web.Security;
using MyUser = ProfitTM.Models.User;

namespace ProfitTM.Controllers
{
    public class UsuariosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            ViewBag.modules = Session["modules"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                switch (Session["prod"].ToString())
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

                ViewBag.data_conn = Session["data_conn"].ToString();
                ViewBag.bran_conn = Session["bran_conn"] != null ? Session["bran_conn"].ToString() : null;

                ViewBag.users = MyUser.GetAllUsers();

                return View();
            }
        }

        public ActionResult Agregar()
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            ViewBag.modules = Session["modules"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                switch (Session["prod"].ToString())
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

                ViewBag.data_conn = Session["data_conn"].ToString();
                ViewBag.bran_conn = Session["bran_conn"] != null ? Session["bran_conn"].ToString() : null;

                ViewBag.allModules = Module.GetAllModules();

                return View();
            }
        }

        public ActionResult Editar(string id)
        {
            ViewBag.user = Session["user"];
            ViewBag.connect = Session["connect"];
            ViewBag.modules = Session["modules"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                switch (Session["prod"].ToString())
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

                ViewBag.data_conn = Session["data_conn"].ToString();
                ViewBag.bran_conn = Session["bran_conn"] != null ? Session["bran_conn"].ToString() : null;

                ViewBag.userEdit = MyUser.GetUserByID(id);
                ViewBag.allModules = Module.GetAllModules();

                return View();
            }
        }
    }
}