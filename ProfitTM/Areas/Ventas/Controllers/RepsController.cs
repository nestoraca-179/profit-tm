using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class RepsController : Controller
    {
        // RepFacturaVentaxArt
        RepFacturaVentaxArt report = new RepFacturaVentaxArt();
        public ActionResult RepFacturaVentaxArtPartial()
        {
            string connect = Session["connect"].ToString();

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepFacturaVentaxArtPartial.cshtml", report);
        }
        public ActionResult RepFacturaVentaxArtPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }

        // RepTotalVentaxArticulo
        RepTotalVentaxArticulo report1 = new RepTotalVentaxArticulo();
        public ActionResult RepTotalVentaxArticuloPartial()
        {
            string connect = Session["connect"].ToString();

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepTotalVentaxArticuloPartial.cshtml", report1);
        }
        public ActionResult RepTotalVentaxArticuloPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report1, Request);
        }

        // RepClienteMasVenta
        RepClienteMasVenta report2 = new RepClienteMasVenta();
        public ActionResult RepClienteMasVentaPartial()
        {
            string connect = Session["connect"].ToString();

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepClienteMasVentaPartial.cshtml", report2);
        }
        public ActionResult RepClienteMasVentaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }
    }
}