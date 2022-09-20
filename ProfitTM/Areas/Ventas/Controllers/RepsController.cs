using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Controllers;
using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Web.Mvc;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class RepsController : Controller
    {
        private readonly static DateTime fecha_h = DateTime.Now;
        private readonly static DateTime fecha_d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // RepFacturaVentaxArt
        RepFacturaVentaxArt report = new RepFacturaVentaxArt();
        public ActionResult RepFacturaVentaxArtPartial()
        {
            string connect = Session["CONNECT"].ToString();
            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report.Parameters["fecDesde"].Value = fecha_d;
            report.Parameters["fecHasta"].Value = fecha_h;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

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
            string connect = Session["CONNECT"].ToString();
            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report1.Parameters["fecDesde"].Value = fecha_d;
            report1.Parameters["fecHasta"].Value = fecha_h;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

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
            string connect = Session["CONNECT"].ToString();
            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report2.Parameters["fecDesde"].Value = fecha_d;
            report2.Parameters["fecHasta"].Value = fecha_h;

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepClienteMasVentaPartial.cshtml", report2);
        }
        public ActionResult RepClienteMasVentaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }

        // RepFormatoFacturaVenta
        RepFormatoFacturaVenta report3 = new RepFormatoFacturaVenta();
        public ActionResult RepFormatoFacturaVentaPartial(string id)
        {
            string connect = Session["CONNECT"].ToString();
            report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["RepFormatoFacturaVenta"].Parameters[0].Value = id;
            ds.Queries["RepFormatoFacturaVenta"].Parameters[1].Value = id;

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepFormatoFacturaVentaPartial.cshtml", report3);
        }
        public ActionResult RepFormatoFacturaVentaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report3, Request);
        }
    }
}