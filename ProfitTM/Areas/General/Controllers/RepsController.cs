using DevExpress.DataAccess.Sql;
using DevExpress.Web.Mvc;
using ProfitTM.Models;
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
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report.LBL_DescEmpresa.Text = conn.Name;
            report.LBL_RIF.Text = conn.RIF;
            report.LBL_Telf.Text = conn.Phone;
            report.LBL_Direc.Text = conn.Address;
            report.Parameters["fecha"].Value = DateTime.Now;

            SqlDataSource ds = report.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report1.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report1.LBL_DescEmpresa.Text = conn.Name;
            report1.LBL_RIF.Text = conn.RIF;
            report1.LBL_Telf.Text = conn.Phone;
            report1.LBL_Direc.Text = conn.Address;
            report1.Parameters["fecDesde"].Value = DateTime.Now;
            report1.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report1.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

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
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report2.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report2.LBL_DescEmpresa.Text = conn.Name;
            report2.LBL_RIF.Text = conn.RIF;
            report2.LBL_Telf.Text = conn.Phone;
            report2.LBL_Direc.Text = conn.Address;
            report2.Parameters["fecDesde"].Value = DateTime.Now;
            report2.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report2.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Areas/General/Views/Reportes/_RepBalanceComprobacionPartial.cshtml", report2);
        }
        public ActionResult RepBalanceComprobacionPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report2, Request);
        }

        // RepEstadoGananciasPerdidas2KDoce
        RepEstadoGananciasPerdidas2KDoce report3 = new RepEstadoGananciasPerdidas2KDoce();
        public ActionResult RepEstadoGananciasPerdidas2KDocePartial()
        {
            string connect = Session["CONNECT"].ToString();
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            report3.PB_Logo.ImageUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + conn.Image;
            report3.LBL_DescEmpresa.Text = conn.Name;
            report3.LBL_RIF.Text = conn.RIF;
            report3.LBL_Telf.Text = conn.Phone;
            report3.LBL_Direc.Text = conn.Address;
            report3.Parameters["fecDesde"].Value = DateTime.Now;
            report3.Parameters["fecHasta"].Value = DateTime.Now;

            SqlDataSource ds = report3.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = "XpoProvider=MSSqlServer;" + connect;

            return PartialView("~/Areas/General/Views/Reportes/_RepEstadoGananciasPerdidas2KDocePartial.cshtml", report3);
        }
        public ActionResult RepEstadoGananciasPerdidas2KDocePartialExport(string name = "")
        {
            return DocumentViewerExtension.ExportTo(report3, Request);

            #region PRUEBAS ENVIO DE CORREO
            //try
            //{
            //    // Generar el reporte utilizando DevExpress Reporting
            //    RepEstadoGananciasPerdidas2KDoce report = GenerateReport();

            //    // Exportar el reporte a PDF
            //    MemoryStream stream = new MemoryStream();
            //    report.ExportToPdf(stream);
            //    stream.Seek(0, SeekOrigin.Begin);

            //    // Adjuntar el archivo PDF al correo electrónico
            //    Attachment attachment = new Attachment(stream, "Report.pdf", "application/pdf");

            //    // Crear el correo electrónico
            //    MailMessage mail = new MailMessage();
            //    mail.From = new MailAddress("acarreno@gtsconsultingca.com");
            //    mail.To.Add("nestoraca.179@gmail.com");
            //    mail.Subject = "Informe Adjunto";
            //    mail.Body = "Adjunto encontrarás el informe solicitado.";
            //    mail.Attachments.Add(attachment);

            //    // Configurar el servidor SMTP saliente
            //    SmtpClient smtpClient = new SmtpClient("mail.gtsconsultingca.com");
            //    smtpClient.Port = 587;
            //    smtpClient.Credentials = new NetworkCredential("acarreno@gtsconsultingca.com", "alfa02-GT$ac*");
            //    smtpClient.EnableSsl = true;
            //    smtpClient.Timeout = 10000;

            //    // Enviar el correo electrónico
            //    smtpClient.Send(mail);

            //    return Content("El informe ha sido enviado por correo electrónico correctamente.");
            //}
            //catch (Exception ex)
            //{
            //    return Content("El informe no pudo ser enviado correctamente: " + ex.Message);
            //}
            #endregion
        }

        #region PRUEBAS ENVIO DE CORREO
        //private RepEstadoGananciasPerdidas2KDoce GenerateReport()
        //{
        //    // Aquí implementa la lógica para generar el reporte utilizando DevExpress Reporting
        //    // Por ejemplo:
        //    RepEstadoGananciasPerdidas2KDoce reporte = new RepEstadoGananciasPerdidas2KDoce();
        //    reporte.Parameters["fecDesde"].Value = DateTime.Now;
        //    reporte.Parameters["fecHasta"].Value = DateTime.Now;
        //    reporte.CreateDocument();
        //    return reporte;
        //}
        #endregion
    }
}