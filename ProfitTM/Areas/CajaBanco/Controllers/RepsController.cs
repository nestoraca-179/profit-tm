using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System.Web.Mvc;

namespace ProfitTM.Areas.CajaBanco.Controllers
{
    public class RepsController : Controller
    {
        // RepBanco
        RepBanco report = new RepBanco();
        public ActionResult RepBancoPartial()
        {
            string connect = Session["CONNECT"].ToString();
            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

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
            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

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
            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

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
            report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/CajaBanco/Views/Reportes/_RepDisponibilidadPartial.cshtml", report3);
        }
        public ActionResult RepDisponibilidadPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report3, Request);
        }
    }
}