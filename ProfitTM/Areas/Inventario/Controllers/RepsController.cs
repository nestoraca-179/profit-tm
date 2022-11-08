using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Models;
using System.Web.Mvc;

namespace ProfitTM.Areas.Inventario.Controllers
{
    public class RepsController : Controller
    {
        // RepArticuloConPrecio
        RepArticuloConPrecio report = new RepArticuloConPrecio();
        public ActionResult RepArticuloConPrecioPartial()
        {
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report.LBL_DescEmpresa.Text = conn.Name;
            report.LBL_RIF.Text = conn.RIF;
            report.LBL_Telf.Text = conn.Phone;
            report.LBL_Direc.Text = conn.Address;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report1.LBL_DescEmpresa.Text = conn.Name;
            report1.LBL_RIF.Text = conn.RIF;
            report1.LBL_Telf.Text = conn.Phone;
            report1.LBL_Direc.Text = conn.Address;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report2.LBL_DescEmpresa.Text = conn.Name;
            report2.LBL_RIF.Text = conn.RIF;
            report2.LBL_Telf.Text = conn.Phone;
            report2.LBL_Direc.Text = conn.Address;

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report3.LBL_DescEmpresa.Text = conn.Name;
            report3.LBL_RIF.Text = conn.RIF;
            report3.LBL_Telf.Text = conn.Phone;
            report3.LBL_Direc.Text = conn.Address;

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report4.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report4.LBL_DescEmpresa.Text = conn.Name;
            report4.LBL_RIF.Text = conn.RIF;
            report4.LBL_Telf.Text = conn.Phone;
            report4.LBL_Direc.Text = conn.Address;

            SqlDataSource ds = report4.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report5.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report5.LBL_DescEmpresa.Text = conn.Name;
            report5.LBL_RIF.Text = conn.RIF;
            report5.LBL_Telf.Text = conn.Phone;
            report5.LBL_Direc.Text = conn.Address;

            SqlDataSource ds = report5.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Areas/Inventario/Views/Reportes/_RepArticuloConCostoYPrecioPartial.cshtml", report5);
        }
        public ActionResult RepArticuloConCostoYPrecioPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report5, Request);
        }
    }
}