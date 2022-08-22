using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.user_conn = Session["user_conn"].ToString();
            ViewBag.data_conn = Session["data_conn"].ToString();
            ViewBag.bran_conn = Session["bran_conn"].ToString();

            return View();
        }
    }
}