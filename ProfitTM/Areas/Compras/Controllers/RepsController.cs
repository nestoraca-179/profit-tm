using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    public class RepsController : Controller
    {
        // RepCompraxArt
        RepCompraxArt report = new RepCompraxArt();
        public ActionResult RepCompraxArtPartial()
        {
            string connect = Session["connect"].ToString();
            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.ConnectionName = connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Compras/Views/Reportes/_RepCompraxArtPartial.cshtml", report);
        }
        public ActionResult RepCompraxArtPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }

        // RepTotalCompraxArticulo
        RepTotalCompraxArticulo report1 = new RepTotalCompraxArticulo();
        public ActionResult RepTotalCompraxArticuloPartial()
        {
            string connect = Session["connect"].ToString();
            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.ConnectionName = connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Compras/Views/Reportes/_RepTotalCompraxArticuloPartial.cshtml", report1);
        }
        public ActionResult RepTotalCompraxArticuloPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report1, Request);
        }

        // RepProveedorMasCompra
        RepProveedorMasCompra report2 = new RepProveedorMasCompra();
        public ActionResult RepProveedorMasCompraPartial()
        {
            string connect = Session["connect"].ToString();
            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.ConnectionName = connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Compras/Views/Reportes/_RepProveedorMasCompraPartial.cshtml", report2);
        }
        public ActionResult RepProveedorMasCompraPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }
    }
}