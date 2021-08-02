using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

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

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

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

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

            return PartialView("~/Areas/Compras/Views/Reportes/_RepProveedorMasCompraPartial.cshtml", report2);
        }
        public ActionResult RepProveedorMasCompraPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }
    }
}