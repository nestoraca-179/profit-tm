using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System;
using System.Web.Mvc;

namespace ProfitTM.Areas.General.Controllers
{
    public class RepsController : Controller
    {
        // RepBalanceGeneral2KDoce
        RepBalanceGeneral2KDoce report = new RepBalanceGeneral2KDoce();
        public ActionResult RepBalanceGeneral2KDocePartial()
        {
            string connect = Session["CONNECT"].ToString();
            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report.Parameters["fecha"].Value = DateTime.Now;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/General/Views/Reportes/_RepBalanceGeneral2KDocePartial.cshtml", report);
        }
        public ActionResult RepBalanceGeneral2KDocePartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }

        // RepEstadoResultados2KDoce
        RepEstadoResultados2KDoce report1 = new RepEstadoResultados2KDoce();
        public ActionResult RepEstadoResultados2KDocePartial()
        {
            string connect = Session["CONNECT"].ToString();
            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report1.Parameters["fecDesde"].Value = DateTime.Now;
            report1.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/General/Views/Reportes/_RepEstadoResultados2KDocePartial.cshtml", report1);
        }
        public ActionResult RepEstadoResultados2KDocePartialExport()
        {
            return DocumentViewerExtension.ExportTo(report1, Request);
        }

        // RepBalanceComprobacion
        RepBalanceComprobacion report2 = new RepBalanceComprobacion();
        public ActionResult RepBalanceComprobacionPartial()
        {
            string connect = Session["CONNECT"].ToString();
            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report2.Parameters["fecDesde"].Value = DateTime.Now;
            report2.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/General/Views/Reportes/_RepBalanceComprobacionPartial.cshtml", report2);
        }
        public ActionResult RepBalanceComprobacionPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }
    }
}