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
            ViewBag.user = Session["USER"];
            ViewBag.connect = Session["CONNECT"];
            ViewBag.modules = Session["MODULES"];

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

                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"] != null ? Session["BRAN_CONN"].ToString() : null;

                ViewBag.users = MyUser.GetAllUsers();

                return View();
            }
        }

        public ActionResult Agregar()
        {
            ViewBag.user = Session["USER"];
            ViewBag.connect = Session["CONNECT"];
            ViewBag.modules = Session["MODULES"];

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

                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"] != null ? Session["BRAN_CONN"].ToString() : null;

                ViewBag.allModules = Module.GetAllModules();

                return View();
            }
        }

        public ActionResult Editar(string id)
        {
            ViewBag.user = Session["USER"];
            ViewBag.connect = Session["CONNECT"];
            ViewBag.modules = Session["MODULES"];

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

                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"] != null ? Session["BRAN_CONN"].ToString() : null;

                ViewBag.userEdit = MyUser.GetUserByID(id);
                ViewBag.allModules = Module.GetAllModules();

                return View();
            }
        }
    }
}