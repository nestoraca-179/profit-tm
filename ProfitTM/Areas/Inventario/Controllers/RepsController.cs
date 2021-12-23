using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System.Web.Mvc;

namespace ProfitTM.Areas.Inventario.Controllers
{
    public class RepsController : Controller
    {
        // RepArticuloConPrecio
        RepArticuloConPrecio report = new RepArticuloConPrecio();
        public ActionResult RepArticuloConPrecioPartial()
        {
            string connect = Session["connect"].ToString();
            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticuloConPrecioPartial.cshtml", report);
        }
        public ActionResult RepArticuloConPrecioPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }

        // RepArticuloProveedor
        RepArticuloProveedor report1 = new RepArticuloProveedor();
        public ActionResult RepArticuloProveedorPartial()
        {
            string connect = Session["connect"].ToString();
            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticuloProveedorPartial.cshtml", report1);
        }
        public ActionResult RepArticuloProveedorPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report1, Request);
        }

        // RepStockArticulos
        RepStockArticulos report2 = new RepStockArticulos();
        public ActionResult RepStockArticulosPartial()
        {
            string connect = Session["connect"].ToString();
            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepStockArticulosPartial.cshtml", report2);
        }
        public ActionResult RepStockArticulosPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }

        // RepArticuloConCosto
        RepArticuloConCosto report3 = new RepArticuloConCosto();
        public ActionResult RepArticuloConCostoPartial()
        {
            string connect = Session["connect"].ToString();
            report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticuloConCostoPartial.cshtml", report3);
        }
        public ActionResult RepArticuloConCostoPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report3, Request);
        }

        // RepArticulosTodoStock
        RepArticulosTodoStock report4 = new RepArticulosTodoStock();
        public ActionResult RepArticulosTodoStockPartial()
        {
            string connect = Session["connect"].ToString();
            report4.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report4.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticulosTodoStockPartial.cshtml", report4);
        }
        public ActionResult RepArticulosTodoStockPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report4, Request);
        }

        // RepArticuloConCostoYPrecio
        RepArticuloConCostoYPrecio report5 = new RepArticuloConCostoYPrecio();
        public ActionResult RepArticuloConCostoYPrecioPartial()
        {
            string connect = Session["connect"].ToString();
            report5.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report5.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticuloConCostoYPrecioPartial.cshtml", report5);
        }
        public ActionResult RepArticuloConCostoYPrecioPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report5, Request);
        }
    }
}