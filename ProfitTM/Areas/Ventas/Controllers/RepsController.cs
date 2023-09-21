using System;
using System.Web.Mvc;
using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Models;

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

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepClienteMasVentaPartial.cshtml", report2);
        }
        public ActionResult RepClienteMasVentaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }

        // RepFormatoFacturaVenta
        RepFormatoFacturaVenta report3 = new RepFormatoFacturaVenta();
        public ActionResult RepFormatoFacturaVentaPartial(string id, bool c)
        {
            string connect = Session["CONNECT"].ToString();
            // report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            report3.Parameters["nroFact"].Value = id;
            report3.nc_title.Visible = c;
            report3.nc_value.Visible = c;
            report3.note.Visible = c;

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepFormatoFacturaVentaPartial.cshtml", report3);
        }
        public ActionResult RepFormatoFacturaVentaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report3, Request);
        }

        RepFormatoFacturaVentaOM report4 = new RepFormatoFacturaVentaOM();
        public ActionResult RepFormatoFacturaVentaOMPartial(string id, bool c)
        {
            string connect = Session["CONNECT"].ToString();
            // report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report4.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            report4.Parameters["nroFact"].Value = id;
            report4.nc_title.Visible = c;
            report4.nc_value.Visible = c;
            report4.note.Visible = c;

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepFormatoFacturaVentaOMPartial.cshtml", report4);
        }
        public ActionResult RepFormatoFacturaVentaOMPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report4, Request);
        }
    }
}