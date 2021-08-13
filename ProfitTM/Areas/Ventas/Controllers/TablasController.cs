using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class TablasController : Controller
    {
        // GET: Ventas/Tablas
        public ActionResult Index()
        {
            ViewBag.user = Session["user"];
            ViewBag.options = Session["options"];

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { area = "", message = "Debes iniciar sesión" });
            }
            else
            {
                return View();
            }
        }
    }
}