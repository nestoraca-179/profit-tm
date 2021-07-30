using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfitTM.Areas.Compras.Controllers
{
    public class RepsController : Controller
    {
        // RepCompraxArt
        RepCompraxArt report = new RepCompraxArt();
        public ActionResult RepCompraxArtPartial()
        {
            string connect = Session["connect"].ToString();

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.ConnectionName = connect;

            return PartialView("~/Areas/Compras/Views/Reportes/_RepCompraxArtPartial.cshtml", report);
        }
        public ActionResult RepCompraxArtPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
    }
}