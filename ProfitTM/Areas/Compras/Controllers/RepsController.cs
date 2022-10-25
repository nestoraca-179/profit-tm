using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Models;
using System;
using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    public class RepsController : Controller
    {
        // RepCompraxArt
        RepCompraxArt report = new RepCompraxArt();
        public ActionResult RepCompraxArtPartial()
        {
            string connect = Session["CONNECT"].ToString();
            string path = Connection.GetConnByID(Session["ID_CONN"].ToString()).Image;

            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + path;
            report.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
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
            string connect = Session["CONNECT"].ToString();
            string path = Connection.GetConnByID(Session["ID_CONN"].ToString()).Image;

            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + path;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
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
            string connect = Session["CONNECT"].ToString();
            string path = Connection.GetConnByID(Session["ID_CONN"].ToString()).Image;

            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + path;

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Compras/Views/Reportes/_RepProveedorMasCompraPartial.cshtml", report2);
        }
        public ActionResult RepProveedorMasCompraPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }
    }
}