using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Models;
using System;
using System.Web.Mvc;

namespace ProfitTM.Areas.Fiscal.Controllers
{
    public class RepsController : Controller
    {
        // RepMayorAnalitico2KDoce
        RepMayorAnalitico2KDoce report = new RepMayorAnalitico2KDoce();
        public ActionResult RepMayorAnalitico2KDocePartial()
        {
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report.LBL_DescEmpresa.Text = conn.Name;
            report.LBL_RIF.Text = conn.RIF;
            report.LBL_Telf.Text = conn.Phone;
            report.LBL_Direc.Text = conn.Address;
            report.Parameters["fecDesde"].Value = DateTime.Now;
            report.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Areas/Fiscal/Views/Reportes/_RepMayorAnalitico2KDocePartial.cshtml", report);
        }
        public ActionResult RepMayorAnalitico2KDocePartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
    }
}