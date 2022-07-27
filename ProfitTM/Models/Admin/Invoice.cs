using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Invoice : ProfitAdmManager
    {
        #region CODIGO ANTERIOR

        /*
        public string ID { get; set; }
        public Person InvoicePerson { get; set; }
        public string Descrip { get; set; }
        public DateTime DateEmis { get; set; }
        public DateTime DateVenc { get; set; }
        public DateTime DateReg { get; set; }
        public string ControlNumber { get; set; }
        public Transport Transport { get; set; }
        public Currency Currency { get; set; }
        public double Rate { get; set; }
        public Seller Seller { get; set; }
        public Cond Cond { get; set; }
        public Account Account { get; set; }
        public int Status { get; set; }
        public bool Canceled { get; set; }
        public bool VenTer { get; set; }
        public double PorcDescGlob { get; set; }
        public double MontDescGlob { get; set; }
        public double PorcReca { get; set; }
        public double MontReca { get; set; }
        public double SubTotal { get; set; }
        public double IVA { get; set; }
        public double IVA2 { get; set; }
        public double IVA3 { get; set; }
        public double Others { get; set; }
        public double Others2 { get; set; }
        public double Others3 { get; set; }
        public double Total { get; set; }
        public double Sald { get; set; }
        public string DelAddress { get; set; }
        public string Comment { get; set; }
        public string DisCen { get; set; }
        public string NumCom { get; set; }
        public DateTime FecCom { get; set; }
        public bool Contrib { get; set; }
        public int Serials { get; set; }
        public string Salestax { get; set; }
        public string ImpFis { get; set; }
        public string ImpFisFac { get; set; }
        public string ImpNroZ { get; set; }
        public bool Printed { get; set; }
        public string CoUsIn { get; set; }
        public string CoUsMo { get; set; }
        public string CoSucuIn { get; set; }
        public string CoSucuMo { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateMo { get; set; }
        public string Reviewed { get; set; }
        public string Trasnfe { get; set; }
        public string Rowguid { get; set; }
        public string Validator { get; set; }
        public string Type { get; set; } // Indica si la factura es de compra o de venta
        public List<string> ExtraFields { get; set; }
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
            string DBadmin = connect;

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
                                    Descrip = reader["descrip"].ToString(),
                                    DateEmis = DateTime.Parse(reader["fec_emis"].ToString()),
                                    DateVenc = DateTime.Parse(reader["fec_venc"].ToString()),
                                    DateReg = DateTime.Parse(reader["fec_reg"].ToString()),
                                    ControlNumber = reader["n_control"].ToString(),
                                    Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
                                    Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
                                    Rate = double.Parse(reader["tasa"].ToString()),
                                    SubTotal = double.Parse(reader["total_bruto"].ToString()),
                                    IVA = double.Parse(reader["monto_imp"].ToString()),
                                    IVA2 = double.Parse(reader["monto_imp2"].ToString()),
                                    IVA3 = double.Parse(reader["monto_imp3"].ToString()),
                                    Others = double.Parse(reader["otros1"].ToString()),
                                    Others2 = double.Parse(reader["otros2"].ToString()),
                                    Others3 = double.Parse(reader["otros3"].ToString()),
                                    Total = double.Parse(reader["total_neto"].ToString()),
                                    Sald = double.Parse(reader["saldo"].ToString()),
                                    DelAddress = reader["dir_ent"].ToString(),
                                    Comment = reader["comentario"].ToString(),
                                    Cond = Cond.GetCond(connect, reader["co_cond"].ToString()),
                                    Status = int.Parse(reader["status"].ToString()),
                                    Canceled = bool.Parse(reader["anulado"].ToString()),
                                    VenTer = bool.Parse(reader["ven_ter"].ToString()),
                                    PorcDescGlob = double.Parse(reader["porc_desc_glob"].ToString()),
                                    MontDescGlob = double.Parse(reader["monto_desc_glob"].ToString()),
                                    PorcReca = double.Parse(reader["porc_reca"].ToString()),
                                    MontReca = double.Parse(reader["monto_reca"].ToString()),
                                    Contrib = bool.Parse(reader["contrib"].ToString()),
                                    DisCen = reader["dis_cen"].ToString(),
                                    FecCom = DateTime.Parse(reader["feccom"].ToString()),
                                    NumCom = reader["numcom"].ToString(),
                                    Serials = int.Parse(reader["seriales_s"].ToString()),
                                    Salestax = reader["salestax"].ToString(),
                                    ImpFis = reader["impfis"].ToString(),
                                    ImpFisFac = reader["impfisfac"].ToString(),
                                    ImpNroZ = reader["imp_nro_z"].ToString(),
                                    Printed = bool.Parse(reader["impresa"].ToString()),
                                    CoUsIn = reader["co_us_in"].ToString(),
                                    CoUsMo = reader["co_us_mo"].ToString(),
                                    CoSucuIn = reader["co_sucu_in"].ToString(),
                                    CoSucuMo = reader["co_sucu_mo"].ToString(),
                                    DateIn = DateTime.Parse(reader["fe_us_in"].ToString()),
                                    DateMo = DateTime.Parse(reader["fe_us_mo"].ToString()),
                                    Type = type,
                                    Items = GetInvoiceItems(connect, reader["doc_num"].ToString(), type)
                                };

                                invoice.ExtraFields = new List<string>();
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
            string DBadmin = connect;

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
                                string PorcDesc = reader["porc_desc_glob"].ToString();
                                string PorcReca = reader["porc_reca"].ToString();
                                string SerialsS = reader["seriales_s"].ToString();

                                Invoice invoice = new Invoice()
                                {
                                    ID = reader["doc_num"].ToString().Trim(),
                                    Descrip = reader["descrip"].ToString(),
                                    DateEmis = DateTime.Parse(reader["fec_emis"].ToString()),
                                    DateVenc = DateTime.Parse(reader["fec_venc"].ToString()),
                                    DateReg = DateTime.Parse(reader["fec_reg"].ToString()),
                                    ControlNumber = reader["n_control"].ToString(),
                                    Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
                                    Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
                                    Rate = double.Parse(reader["tasa"].ToString()),
                                    SubTotal = double.Parse(reader["total_bruto"].ToString()),
                                    IVA = double.Parse(reader["monto_imp"].ToString()),
                                    IVA2 = double.Parse(reader["monto_imp2"].ToString()),
                                    IVA3 = double.Parse(reader["monto_imp3"].ToString()),
                                    Others = double.Parse(reader["otros1"].ToString()),
                                    Others2 = double.Parse(reader["otros2"].ToString()),
                                    Others3 = double.Parse(reader["otros3"].ToString()),
                                    Total = double.Parse(reader["total_neto"].ToString()),
                                    Sald = double.Parse(reader["saldo"].ToString()),
                                    DelAddress = reader["dir_ent"].ToString(),
                                    Comment = reader["comentario"].ToString(),
                                    Cond = Cond.GetCond(connect, reader["co_cond"].ToString()),
                                    Status = int.Parse(reader["status"].ToString()),
                                    Canceled = bool.Parse(reader["anulado"].ToString()),
                                    VenTer = bool.Parse(reader["ven_ter"].ToString()),
                                    PorcDescGlob = double.Parse(PorcDesc == "" ? "0" : PorcDesc),
                                    MontDescGlob = double.Parse(reader["monto_desc_glob"].ToString()),
                                    PorcReca = double.Parse(PorcReca == "" ? "0" : PorcReca),
                                    MontReca = double.Parse(reader["monto_reca"].ToString()),
                                    Contrib = bool.Parse(reader["contrib"].ToString()),
                                    Serials = int.Parse(SerialsS == "" ? "0" : SerialsS),
                                    Salestax = reader["salestax"].ToString(),
                                    ImpFis = reader["impfis"].ToString(),
                                    ImpFisFac = reader["impfisfac"].ToString(),
                                    ImpNroZ = reader["imp_nro_z"].ToString(),
                                    Printed = bool.Parse(reader["impresa"].ToString()),
                                    CoUsIn = reader["co_us_in"].ToString(),
                                    CoUsMo = reader["co_us_mo"].ToString(),
                                    CoSucuIn = reader["co_sucu_in"].ToString(),
                                    CoSucuMo = reader["co_sucu_mo"].ToString(),
                                    DateIn = DateTime.Parse(reader["fe_us_in"].ToString()),
                                    DateMo = DateTime.Parse(reader["fe_us_mo"].ToString()),
                                    Type = type,
                                    Items = GetInvoiceItems(connect, reader["doc_num"].ToString(), type)
                                };

                                invoice.ExtraFields = new List<string>();
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
            string DBadmin = connect, proc = "";

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
                                    Model = reader["modelo"].ToString(),
                                    Storage = Storage.GetStorage(connect, reader["co_alma"].ToString()),
                                    Unit = reader["co_uni"].ToString().Trim(),
                                    Quantity = double.Parse(reader["total_art"].ToString()),
                                    TipoImp = reader["tipo_imp"].ToString(),
                                    TipoImp2 = reader["tipo_imp2"].ToString(),
                                    TipoImp3 = reader["tipo_imp3"].ToString(),
                                    PorcImp = double.Parse(reader["porc_imp"].ToString()),
                                    PorcImp2 = double.Parse(reader["porc_imp2"].ToString()),
                                    PorcImp3 = double.Parse(reader["porc_imp3"].ToString()),
                                    MontImp = double.Parse(reader["monto_imp"].ToString()),
                                    MontImp2 = double.Parse(reader["monto_imp2"].ToString()),
                                    MontImp3 = double.Parse(reader["monto_imp3"].ToString()),
                                    Amount = double.Parse(reader["reng_neto"].ToString()),
                                    PorcDesc = reader["porc_desc"].ToString(),
                                    MontDesc = double.Parse(reader["monto_desc"].ToString()),
                                    TypeArt = InvoiceItem.GetTypeItem(connect, reader["co_art"].ToString()),
                                    Rowguid = reader["rowguid"].ToString(),
                                    Pend = double.Parse(reader["pendiente"].ToString()),
                                    Pend2 = double.Parse(reader["pendiente2"].ToString()),
                                    TipDoc = reader["tipo_doc"].ToString(),
                                    NumDoc = reader["num_doc"].ToString(),
                                    RowguidDoc = reader["rowguid_doc"].ToString(),
                                    TotDev = double.Parse(reader["total_dev"].ToString()),
                                    MontDev = double.Parse(reader["monto_dev"].ToString()),
                                    Others = double.Parse(reader["otros"].ToString()),
                                    Comment = reader["comentario"].ToString(),
                                    LotAsign = bool.Parse(reader["lote_asignado"].ToString()),
                                    MontDescGlob = double.Parse(reader["monto_desc_glob"].ToString()),
                                    MontRecaGlob = double.Parse(reader["monto_reca_glob"].ToString()),
                                    Others1Glob = double.Parse(reader["otros1_glob"].ToString()),
                                    Others2Glob = double.Parse(reader["otros2_glob"].ToString()),
                                    Others3Glob = double.Parse(reader["otros3_glob"].ToString()),
                                    MontImpAfecGlob = double.Parse(reader["monto_imp_afec_glob"].ToString()),
                                    MontImp2AfecGlob = double.Parse(reader["monto_imp2_afec_glob"].ToString()),
                                    MontImp3AfecGlob = double.Parse(reader["monto_imp3_afec_glob"].ToString()),
                                    DisCen = reader["dis_cen"].ToString(),
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
        */

        #endregion

        public static saFacturaVenta GetSaleInvoice(string id)
        {
            saFacturaVenta invoice = new saFacturaVenta();

            try
            {
                using (db)
                {
                    invoice = db.saFacturaVenta.SingleOrDefault(i => i.doc_num == id);
                }
            }
            catch (Exception ex)
            {
                invoice = null;
            }

            return invoice;
        }

        public List<saFacturaVenta> GetAllSaleInvoices()
        {
            List<saFacturaVenta> invoices = new List<saFacturaVenta>();

            try
            {
                invoices = db.saFacturaVenta.ToList();
            }
            catch (Exception ex)
            {
                invoices = null;
            }

            return invoices;
        }

        public static saFacturaCompra GetBuyInvoice(string id)
        {
            saFacturaCompra invoice = new saFacturaCompra();

            try
            {
                using (db)
                {
                    invoice = db.saFacturaCompra.SingleOrDefault(i => i.doc_num == id);
                }
            }
            catch (Exception ex)
            {
                invoice = null;
            }

            return invoice;
        }

        public List<saFacturaCompra> GetAllBuyInvoices()
        {
            List<saFacturaCompra> invoices = new List<saFacturaCompra>();

            try
            {
                invoices = db.saFacturaCompra.ToList();
            }
            catch (Exception ex)
            {
                invoices = null;
            }

            return invoices;
        }
    
        public object GetStatsInvoices(DateTime fec_d, DateTime fec_h)
        {
            int totalCount = 0;
            decimal totalAmountSale = 0, totalAmountPurchase = 0, totalState = 0;

            // VENTAS
            var sp1 = db.RepFacturaVentaxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            var enumerator1 = sp1.GetEnumerator();

            while (enumerator1.MoveNext())
            {
                totalCount++;
                totalAmountSale += Math.Round(decimal.Parse(enumerator1.Current.total_neto.ToString()), 2);
            }

            // COMPRAS
            var sp2 = db.RepCompraxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null);
            var enumerator2 = sp2.GetEnumerator();

            while (enumerator2.MoveNext())
            {
                totalAmountPurchase += Math.Round(decimal.Parse(enumerator2.Current.total_neto.ToString()), 2);
            }

            // ESTADO DE GANANCIA
            totalState = totalAmountSale - totalAmountPurchase;

            enumerator1.Dispose();
            enumerator2.Dispose();

            // OBJETO ESTADISTICAS
            var obj = new {
                totalCount = totalCount,
                totalAmountSale = totalAmountSale,
                totalAmountPurchase = totalAmountPurchase,
                totalState = totalState
            };

            return obj;
        }
    }
}