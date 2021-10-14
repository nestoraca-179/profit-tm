using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Invoice
    {
        public string ID { get; set; }
        public Person InvoicePerson { get; set; }
        public DateTime DateEmis { get; set; }
        public DateTime DateVenc { get; set; }
        public string ControlNumber { get; set; }
        public Transport Transport { get; set; }
        public Currency Currency { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
        public bool Printed { get; set; }
        public string Type { get; set; }
        public List<InvoiceItem> Items { get; set; }

        public static List<Invoice> GetAllInvoices(string connect, string type)
        {
            List<Invoice> invoices = new List<Invoice>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            string name = "", table = "";
            bool invoiceSale = false;

            switch (type)
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

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(string.Format("SELECT * FROM {0} ORDER BY fec_emis DESC", table), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Invoice invoice = new Invoice()
                                {
                                    ID = reader["doc_num"].ToString().Trim(),
                                    DateEmis = Convert.ToDateTime(reader["fec_emis"].ToString()),
                                    DateVenc = Convert.ToDateTime(reader["fec_venc"].ToString()),
                                    ControlNumber = reader["n_control"].ToString(),
                                    Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
                                    Rate = double.Parse(reader["tasa"].ToString()),
                                    Amount = double.Parse(reader["total_neto"].ToString()),
                                    Status = int.Parse(reader["status"].ToString()),
                                    Printed = bool.Parse(reader["impresa"].ToString()),
                                    Type = type,
                                    Items = GetInvoiceItems(connect, reader["doc_num"].ToString(), type)
                                };

                                string ID = reader[name].ToString();

                                if (invoiceSale)
                                {
                                    invoice.InvoicePerson = Client.GetClient(connect, ID);
                                    invoice.Transport = Transport.GetTransport(connect, reader["co_tran"].ToString());
                                }
                                else
                                {
                                    invoice.InvoicePerson = Supplier.GetSupplier(connect, ID);
                                }

                                invoices.Add(invoice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                invoices = null;
            }

            return invoices;
        }

        public static List<InvoiceItem> GetInvoiceItems(string connect, string id, string type)
        {
            List<InvoiceItem> items = new List<InvoiceItem>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString, proc = "";

            switch (type)
            {
                case "V":
                    proc = "pSeleccionarRenglonesFacturaVenta";

                    break;
                case "C":
                    proc = "pSeleccionarRenglonesFacturaCompra";

                    break;
                case "PV":
                    proc = "pSeleccionarRenglonesPlantillaVenta";

                    break;
                case "PC":
                    proc = "pSeleccionarRenglonesPlantillaCompra";

                    break;
            }

            string query = string.Format("exec {0} @sDoc_Num = '{1}'", proc, id);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new InvoiceItem()
                                {
                                    Reng = reader["reng_num"].ToString(),
                                    Code = reader["co_art"].ToString().Trim(),
                                    Name = reader["art_des"].ToString(),
                                    Storage = Storage.GetStorage(connect, reader["co_alma"].ToString()),
                                    Quantity = double.Parse(reader["total_art"].ToString()),
                                    Amount = double.Parse(reader["reng_neto"].ToString()),
                                    IVA = double.Parse(reader["monto_imp"].ToString()),
                                    TypeArt = InvoiceItem.GetTypeItem(connect, reader["co_art"].ToString())
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                items = null;
            }

            return items;
        }
    }
}