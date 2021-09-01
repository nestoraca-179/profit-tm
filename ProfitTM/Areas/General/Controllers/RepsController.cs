using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfitTM.Areas.General.Controllers
{
    public class RepsController : Controller
    {
        // RepBalanceGeneral2KDoce
        RepBalanceGeneral2KDoce report = new RepBalanceGeneral2KDoce();
        public ActionResult RepBalanceGeneral2KDocePartial()
        {
            string connect = Session["connect"].ToString();
            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.ConnectionName = connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/General/Views/Reportes/_RepBalanceGeneral2KDocePartial.cshtml", report);
        }
        public ActionResult RepBalanceGeneral2KDocePartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
    }
}