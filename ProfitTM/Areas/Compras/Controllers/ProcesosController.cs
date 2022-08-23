using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.product = "Administrativo";
            ViewBag.data_conn = Session["data_conn"].ToString();
            ViewBag.bran_conn = Session["bran_conn"].ToString();

            return View();
        }
    }
}