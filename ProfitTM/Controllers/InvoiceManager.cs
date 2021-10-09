using ProfitTM.Models;
using System;
using System.Collections.Generic;
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

        public ProfitTMResponse editInvoice(Invoice invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            string name = "", table = "";
            bool invoiceSale = false;

            switch (invoice.Type)
            {
                case "V":
                    invoiceSale = true;
                    name = "co_cli";
                    table = "saFacturaVenta";

                    break;
                case "C":
                    name = "co_prov";
                    table = "saFacturaCompra";

                    break;
                case "PV":
                    invoiceSale = true;
                    name = "co_cli";
                    table = "saPlantillaVenta";

                    break;
                case "PC":
                    name = "co_prov";
                    table = "saPlantillaCompra";

                    break;
            }

            query.Append(string.Format("UPDATE {0} ", table));
            query.Append(string.Format("SET {0} = '{1}', ", name, invoice.InvoicePerson.ID));

            if (invoiceSale)
            {
                query.Append(string.Format("co_cond = '{0}', ", invoice.InvoicePerson.Cond.ID));
                query.Append(string.Format("co_ven = '{0}', ", invoice.InvoicePerson.Seller.ID));
                query.Append(string.Format("co_tran = '{0}', ", invoice.Transport.ID));
            }

            query.Append(string.Format("n_control = '{0}' ", invoice.ControlNumber));
            query.Append(string.Format("WHERE doc_num = '" + invoice.ID + "'"));

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        int rows = comm.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            response.Status = "OK";
                            response.Result = rows;
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