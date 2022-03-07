using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Controllers;
using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Web.Mvc;

namespace ProfitTM.Areas.Ventas.Controllers
{
    public class RepsController : Controller
    {
        // RepFacturaVentaxArt
        RepFacturaVentaxArt report = new RepFacturaVentaxArt();
        public ActionResult RepFacturaVentaxArtPartial()
        {
            string connect = Session["connect"].ToString();
            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report.Parameters["fecDesde"].Value = new DateTime(2000, 01, 01);
            report.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

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
            string connect = Session["connect"].ToString();
            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report1.Parameters["fecDesde"].Value = new DateTime(2000, 01, 01);
            report1.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

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
            string connect = Session["connect"].ToString();
            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";
            report2.Parameters["fecDesde"].Value = new DateTime(2000, 01, 01);
            report2.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["Master"].Parameters[0].Value = Session["DB"].ToString();

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepClienteMasVentaPartial.cshtml", report2);
        }
        public ActionResult RepClienteMasVentaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }

        // RepFormatoFacturaVenta
        RepFormatoFacturaVenta report3 = new RepFormatoFacturaVenta();
        public ActionResult RepFormatoFacturaVentaPartial(string id)
        {
            string connect = Session["connect"].ToString();
            report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/images/Logo-prod.png";

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;
            ds.Queries["RepFormatoFacturaVenta"].Parameters[0].Value = id;
            ds.Queries["RepFormatoFacturaVenta"].Parameters[1].Value = id;

            return PartialView("~/Areas/Ventas/Views/Reportes/_RepFormatoFacturaVentaPartial.cshtml", report3);
        }
        public ActionResult RepFormatoFacturaVentaPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report3, Request);
        }

        // GRIDVIEW
        [ValidateInput(false)]
        public ActionResult GridViewFactRengsPartial(string doc_num)
        {
            string connect = Session["connect"].ToString();
            EntityConnectionStringBuilder entity = EntityController.GetEntity(connect);
            ProfitAdmEntities db = new ProfitAdmEntities(entity.ToString());

            List<saFacturaVentaReng> model = db.saFacturaVentaReng.Where(r => r.doc_num == doc_num).ToList();
            return PartialView("~/Areas/Ventas/Views/Procesos/_GridViewFactRengsPartial.cshtml", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewFactRengsPartialAddNew(saFacturaVentaReng item)
        {
            string connect = Session["connect"].ToString();
            EntityConnectionStringBuilder entity = EntityController.GetEntity(connect);
            ProfitAdmEntities db = new ProfitAdmEntities(entity.ToString());

            List<saFacturaVentaReng> model = db.saFacturaVentaReng.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Areas/Ventas/Views/Procesos/_GridViewFactRengsPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewFactRengsPartialUpdate(saFacturaVentaReng item)
        {
            string connect = Session["connect"].ToString();
            EntityConnectionStringBuilder entity = EntityController.GetEntity(connect);
            ProfitAdmEntities db = new ProfitAdmEntities(entity.ToString());

            List<saFacturaVentaReng> model = db.saFacturaVentaReng.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.reng_num == item.reng_num);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Areas/Ventas/Views/Procesos/_GridViewFactRengsPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewFactRengsPartialDelete(Int32 reng_num)
        {
            string connect = Session["connect"].ToString();
            EntityConnectionStringBuilder entity = EntityController.GetEntity(connect);
            ProfitAdmEntities db = new ProfitAdmEntities(entity.ToString());

            List<saFacturaVentaReng> model = db.saFacturaVentaReng.ToList();
            if (reng_num >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.reng_num == reng_num);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Areas/Ventas/Views/Procesos/_GridViewFactRengsPartial.cshtml", model.ToList());
        }
    }
}