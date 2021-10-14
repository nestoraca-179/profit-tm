using ProfitTM.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace ProfitTM.Controllers
{
    public class InvoiceManager
    {
        public string connect { get; set; }
        public string DBadmin { get; set; }

        public InvoiceManager()
        {
            this.connect = HttpContext.Current.Session["connect"].ToString();
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        public InvoiceManager(string connect)
        {
            this.connect = connect;
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        public ProfitTMResponse addInvoice(Invoice invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            return response;
        }

        public ProfitTMResponse editInvoice(Invoice invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder queryF = new StringBuilder(), queryD = new StringBuilder();

            string name = "", tableF = "", tableD = "";
            bool invoiceSale = false;

            switch (invoice.Type)
            {
                case "V":
                    invoiceSale = true;
                    name = "co_cli";
                    tableF = "saFacturaVenta";
                    tableD = "saDocumentoVenta";

                    break;
                case "C":
                    name = "co_prov";
                    tableF = "saFacturaCompra";
                    tableD = "saDocumentoCompra";

                    break;
                case "PV":
                    invoiceSale = true;
                    name = "co_cli";
                    tableF = "saPlantillaVenta";

                    break;
                case "PC":
                    name = "co_prov";
                    tableF = "saPlantillaCompra";

                    break;
            }

            invoice.InvoicePerson.Cond = Cond.GetCond(connect, invoice.InvoicePerson.Cond.ID);
            invoice.DateVenc = invoice.DateEmis.AddDays(invoice.InvoicePerson.Cond.DaysCred);

            queryF.Append(string.Format("UPDATE {0} ", tableF));
            queryF.Append(string.Format("SET {0} = '{1}', ", name, invoice.InvoicePerson.ID));

            if (invoiceSale)
            {
                queryF.Append(string.Format("co_cond = '{0}', ", invoice.InvoicePerson.Cond.ID));
                queryF.Append(string.Format("co_ven = '{0}', ", invoice.InvoicePerson.Seller.ID));
                queryF.Append(string.Format("co_tran = '{0}', ", invoice.Transport.ID));
            }

            queryF.Append(string.Format("tasa = {0}, ", invoice.Rate.ToString().Replace(",", ".")));
            queryF.Append(string.Format("n_control = '{0}', ", invoice.ControlNumber));
            queryF.Append(string.Format("fec_venc = '{0}' ", invoice.DateVenc.ToString("yyyy/MM/dd HH:mm")));
            queryF.Append(string.Format("WHERE doc_num = '" + invoice.ID + "'"));

            if (tableD != "")
            {
                queryD.Append(string.Format("UPDATE {0} ", tableD));
                queryD.Append(string.Format("SET {0} = '{1}', ", name, invoice.InvoicePerson.ID));

                if (invoiceSale)
                {
                    queryD.Append(string.Format("co_ven = '{0}', ", invoice.InvoicePerson.Seller.ID));
                    queryD.Append(string.Format("n_control = '{0}', ", invoice.ControlNumber));
                }

                queryD.Append(string.Format("tasa = {0}, ", invoice.Rate.ToString().Replace(",", ".")));
                queryD.Append(string.Format("fec_venc = '{0}' ", invoice.DateVenc.ToString("yyyy/MM/dd HH:mm")));
                queryD.Append(string.Format("WHERE co_tipo_doc = 'FACT' AND nro_doc = '" + invoice.ID + "'"));
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(queryF.ToString(), conn))
                    {
                        int rowsF = comm.ExecuteNonQuery();
                        comm.CommandText = queryD.ToString();
                        int rowsD = comm.ExecuteNonQuery();

                        if (rowsF > 0 && rowsD > 0)
                        {
                            response.Status = "OK";
                            response.Result = rowsF + rowsD;
                        }
                        else
                        {
                            response.Status = "ERROR";
                            response.Message = "Se ha producido un error al ejecutar la sentencia SQL";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }
    }
}