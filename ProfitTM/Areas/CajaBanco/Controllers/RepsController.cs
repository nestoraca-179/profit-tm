using System;
using System.Web.Mvc;
using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Models;

namespace ProfitTM.Areas.CajaBanco.Controllers
{
    public class RepsController : Controller
    {
        // RepBanco
        RepBanco report = new RepBanco();
        public ActionResult RepBancoPartial()
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

            return PartialView("~/Areas/CajaBanco/Views/Reportes/_RepBancoPartial.cshtml", report);
        }
        public ActionResult RepBancoPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }

        // RepDisponibilidadBancaria
        RepDisponibilidadBancaria report1 = new RepDisponibilidadBancaria();
        public ActionResult RepDisponibilidadBancariaPartial()
        {
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report1.LBL_DescEmpresa.Text = conn.Name;
            report1.LBL_RIF.Text = conn.RIF;
            report1.LBL_Telf.Text = conn.Phone;
            report1.LBL_Direc.Text = conn.Address;
            report1.Parameters["fecha"].Value = DateTime.Now;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Areas/CajaBanco/Views/Reportes/_RepDisponibilidadBancariaPartial.cshtml", report1);
        }
        public ActionResult RepDisponibilidadBancariaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report1, Request);
        }

        // RepMoviBancoXNum
        RepMoviBancoXNum report2 = new RepMoviBancoXNum();
        public ActionResult RepMoviBancoXNumPartial()
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

            return PartialView("~/Areas/CajaBanco/Views/Reportes/_RepMoviBancoXNumPartial.cshtml", report2);
        }
        public ActionResult RepMoviBancoXNumPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }

        // RepDisponibilidad
        RepDisponibilidad report3 = new RepDisponibilidad();
        public ActionResult RepDisponibilidadPartial()
        {
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report3.LBL_DescEmpresa.Text = conn.Name;
            report3.LBL_RIF.Text = conn.RIF;
            report3.LBL_Telf.Text = conn.Phone;
            report3.LBL_Direc.Text = conn.Address;
            report3.Parameters["fecha"].Value = DateTime.Now;

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Areas/CajaBanco/Views/Reportes/_RepDisponibilidadPartial.cshtml", report3);
        }
        public ActionResult RepDisponibilidadPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report3, Request);
        }

        // RepFormatoOrdenPago
        RepFormatoOrdenPago report4 = new RepFormatoOrdenPago();
        public ActionResult RepFormatoOrdenPagoPartial(string id)
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
            report4.Parameters["nroOrd"].Value = id;

            return PartialView("~/Areas/CajaBanco/Views/Reportes/_RepFormatoOrdenPagoPartial.cshtml", report4);
        }
        public ActionResult RepFormatoOrdenPagoPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report4, Request);
        }

        RepFormatoPago report5 = new RepFormatoPago();
        public ActionResult RepFormatoPagoPartial(string id)
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
            report5.Parameters["nroPag"].Value = id;

            return PartialView("~/Areas/CajaBanco/Views/Reportes/_RepFormatoPagoPartial.cshtml", report5);
        }
        public ActionResult RepFormatoPagoPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report5, Request);
        }
    }
}