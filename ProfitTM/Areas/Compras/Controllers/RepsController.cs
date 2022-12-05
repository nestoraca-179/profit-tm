using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Models;
using System;
using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    public class RepsController : Controller
    {
        private readonly static DateTime fecha_h = DateTime.Now;
        private readonly static DateTime fecha_d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // RepCompraxArt
        RepCompraxArt report = new RepCompraxArt();
        public ActionResult RepCompraxArtPartial()
        {
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report.LBL_DescEmpresa.Text = conn.Name;
            report.LBL_RIF.Text = conn.RIF;
            report.LBL_Telf.Text = conn.Phone;
            report.LBL_Direc.Text = conn.Address;
            report.Parameters["fecDesde"].Value = fecha_d;
            report.Parameters["fecHasta"].Value = fecha_h;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report1.LBL_DescEmpresa.Text = conn.Name;
            report1.LBL_RIF.Text = conn.RIF;
            report1.LBL_Telf.Text = conn.Phone;
            report1.LBL_Direc.Text = conn.Address;
            report1.Parameters["fecDesde"].Value = fecha_d;
            report1.Parameters["fecHasta"].Value = fecha_h;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report2.LBL_DescEmpresa.Text = conn.Name;
            report2.LBL_RIF.Text = conn.RIF;
            report2.LBL_Telf.Text = conn.Phone;
            report2.LBL_Direc.Text = conn.Address;
            report2.Parameters["fecDesde"].Value = fecha_d;
            report2.Parameters["fecHasta"].Value = fecha_h;

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Areas/Compras/Views/Reportes/_RepProveedorMasCompraPartial.cshtml", report2);
        }
        public ActionResult RepProveedorMasCompraPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }
    }
}