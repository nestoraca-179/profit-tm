using ProfitTM.Models;
using ProfitTM.Models.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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

        public ProfitTMResponse getAllInvoices()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Invoice> invoices = new List<Invoice>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("SELECT doc_num, co_cli, fec_emis, total_neto, status, impresa FROM saFacturaVenta", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Invoice invoice = new Invoice()
                                {
                                    ID = reader["doc_num"].ToString(),
                                    Date = DateTime.Parse(reader["fec_emis"].ToString()),
                                    Amount = double.Parse(reader["total_neto"].ToString()),
                                    Status = int.Parse(reader["status"].ToString()),
                                    Printed = bool.Parse(reader["impresa"].ToString()),
                                    Type = 'V'
                                };

                                string clientID = reader["co_cli"].ToString();
                                ProfitTMResponse getName = new ClientManager().searchClient(clientID);

                                if (getName.Status == "OK")
                                    invoice.Descrip = ((Client)getName.Result).Name;
                                else
                                    invoice.Descrip = clientID;

                                invoices.Add(invoice);
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = invoices;
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