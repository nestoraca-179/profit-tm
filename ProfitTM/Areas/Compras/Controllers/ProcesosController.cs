using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    [Authorize]
    public class ProcesosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}