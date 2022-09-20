using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.product = "Administrativo";
            ViewBag.data_conn = Session["DATA_CONN"].ToString();
            ViewBag.bran_conn = Session["BRAN_CONN"].ToString();

            return View();
        }
    }
}