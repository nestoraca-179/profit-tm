using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

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

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

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

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.ConnectionName = connect;
            
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

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticuloConCostoPartial.cshtml", report3);
        }
        public ActionResult RepArticuloConCostoPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report3, Request);
        }

        RepArticulosTodoStock report4 = new RepArticulosTodoStock();
        public ActionResult RepArticulosTodoStockPartial()
        {
            string connect = Session["connect"].ToString();

            SqlDataSource ds = report4.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticulosTodoStockPartial.cshtml", report4);
        }
        public ActionResult RepArticulosTodoStockPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report4, Request);
        }
    }
}