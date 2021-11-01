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
        public double SubTotal { get; set; }
        public double IVA { get; set; }
        public double Amount { get; set; }
        public Seller Seller { get; set; }
        public Cond Cond { get; set; }
        public int Status { get; set; }
        public bool Printed { get; set; }
        public List<string> ExtraFields { get; set; }
        public string UserIn { get; set; } // Codigo usuario que ingreso la factura
        public string BranchIn { get; set; } // Codigo sucursal que ingreso la factura
        public string Type { get; set; } // Indica si la factura es de compra o de venta
        public List<InvoiceItem> Items { get; set; }

        public static Invoice GetInvoice(string connect, string ID, string type)
        {
            Invoice invoice;

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
                    table = "saPedidoVenta";

                    break;
                case "PC":
                    name = "co_prov";
                    table = "saPlantillaCompra";

                    break;
            }

            string query = string.Format("SELECT * FROM {0} WHERE doc_num = '{1}'", table, ID);
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                invoice = new Invoice()
                                {
                                    ID = reader["doc_num"].ToString().Trim(),
                                    DateEmis = Convert.ToDateTime(reader["fec_emis"].ToString()),
                                    DateVenc = Convert.ToDateTime(reader["fec_venc"].ToString()),
                                    ControlNumber = reader["n_control"].ToString(),
                                    Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
                                    Rate = double.Parse(reader["tasa"].ToString()),
                                    SubTotal = double.Parse(reader["total_bruto"].ToString()),
                                    IVA = double.Parse(reader["monto_imp"].ToString()),
                                    Amount = double.Parse(reader["total_neto"].ToString()),
                                    Cond = Cond.GetCond(connect, reader["co_cond"].ToString()),
                                    Status = int.Parse(reader["status"].ToString()),
                                    Printed = bool.Parse(reader["impresa"].ToString()),
                                    UserIn = reader["co_us_in"].ToString().Trim(),
                                    BranchIn = reader["co_sucu_in"].ToString().Trim(),
                                    Type = type,
                                    Items = GetInvoiceItems(connect, reader["doc_num"].ToString(), type)
                                };

                                for (int i = 1; i < 9; i++)
                                {
                                    invoice.ExtraFields.Add(reader["campo" + i].ToString());
                                }

                                string PersonID = reader[name].ToString();

                                if (invoiceSale)
                                {
                                    invoice.InvoicePerson = Client.GetClient(connect, PersonID);
                                    invoice.Seller = Seller.GetSeller(connect, reader["co_ven"].ToString());
                                    invoice.Transport = Transport.GetTransport(connect, reader["co_tran"].ToString());
                                }
                                else
                                {
                                    invoice.InvoicePerson = Supplier.GetSupplier(connect, PersonID);
                                }
                            }
                            else
                            {
                                invoice = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                invoice = null;
            }

            return invoice;
        }

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
                    table = "saPedidoVenta";

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
                    using (SqlCommand comm = new SqlCommand(string.Format("SELECT * FROM {0} ORDER BY fec_reg DESC", table), conn))
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
                                    SubTotal = double.Parse(reader["total_bruto"].ToString()),
                                    IVA = double.Parse(reader["monto_imp"].ToString()),
                                    Amount = double.Parse(reader["total_neto"].ToString()),
                                    Cond = Cond.GetCond(connect, reader["co_cond"].ToString()),
                                    Status = int.Parse(reader["status"].ToString()),
                                    Printed = bool.Parse(reader["impresa"].ToString()),
                                    UserIn = reader["co_us_in"].ToString().Trim(),
                                    BranchIn = reader["co_sucu_in"].ToString().Trim(),
                                    Type = type,
                                    Items = GetInvoiceItems(connect, reader["doc_num"].ToString(), type)
                                };

                                string ID = reader[name].ToString();

                                if (invoiceSale)
                                {
                                    invoice.InvoicePerson = Client.GetClient(connect, ID);
                                    invoice.Seller = Seller.GetSeller(connect, reader["co_ven"].ToString());
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
                    proc = "pSeleccionarRenglonesPedidoVenta";

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
                            InvoiceItem item;

                            while (reader.Read())
                            {
                                item = new InvoiceItem()
                                {
                                    Reng = reader["reng_num"].ToString(),
                                    Code = reader["co_art"].ToString().Trim(),
                                    Name = reader["art_des"].ToString(),
                                    Storage = Storage.GetStorage(connect, reader["co_alma"].ToString()),
                                    Unit = reader["co_uni"].ToString().Trim(),
                                    Quantity = double.Parse(reader["total_art"].ToString()),
                                    ImpCode = reader["tipo_imp"].ToString(),
                                    ImpPorc = reader["porc_imp"].ToString(),
                                    Amount = double.Parse(reader["reng_neto"].ToString()),
                                    IVA = double.Parse(reader["monto_imp"].ToString()),
                                    TypeArt = InvoiceItem.GetTypeItem(connect, reader["co_art"].ToString()),
                                    Rowguid = reader["rowguid"].ToString()
                                };

                                if (type.Contains("V"))
                                    item.PriceCode = reader["co_precio"].ToString().Trim();

                                items.Add(item);
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