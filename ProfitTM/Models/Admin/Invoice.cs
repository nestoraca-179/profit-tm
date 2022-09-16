using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    invoice.saFacturaVentaReng = db.saFacturaVentaReng.Where(r => r.doc_num.Trim() == invoice.doc_num.Trim()).ToList();
                }
            }
            catch (Exception ex)
            {
                invoice = null;
            }

            return invoice;
        }

        public List<saFacturaVenta> GetAllSaleInvoices(string sucur)
        {
            List<saFacturaVenta> invoices = new List<saFacturaVenta>();

            try
            {
                invoices = db.saFacturaVenta.Where(r => r.co_sucu_in == sucur).OrderByDescending(i => i.fec_reg).ThenBy(i => i.doc_num).Take(200).ToList();
                foreach (saFacturaVenta invoice in invoices)
                {
                    invoice.saFacturaVentaReng = new InvoiceItem().GetRengsBySaleInvoice(invoice.doc_num.Trim());
                }
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
                    invoice.saFacturaCompraReng = db.saFacturaCompraReng.Where(r => r.doc_num.Trim() == invoice.doc_num.Trim()).ToList();
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
                invoices = db.saFacturaCompra.Take(200).ToList();
                foreach (saFacturaCompra invoice in invoices)
                {
                    invoice.saFacturaCompraReng = new InvoiceItem().GetRengsByBuyInvoice(invoice.doc_num.Trim());
                }
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

        public saFacturaVenta AddFromOrder(saFacturaVenta invoice, string user, string sucur)
        {
            saFacturaVenta newInvoice = new saFacturaVenta();

            using (DbContextTransaction tran = db.Database.BeginTransaction())
            {
                try
                {
                    string n_fact = "", n_cont = "";
                    foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
                    {
                        db.pStockPendienteActualizar(reng.rowguid, reng.total_art, "PCLI");
                    }

                    var sp_n_fact = db.pConsecutivoProximo(sucur, "FACT_VTA_N_CON").GetEnumerator();
                    var sp_n_cont = db.pConsecutivoProximo(sucur, "DOC_VEN_FACT").GetEnumerator();

                    if (sp_n_fact.MoveNext())
                        n_fact = sp_n_fact.Current;

                    if (sp_n_cont.MoveNext())
                        n_cont = sp_n_cont.Current;

                    var sp = db.pInsertarFacturaVenta(n_fact, invoice.descrip, invoice.co_cli, invoice.co_tran, invoice.co_mone, invoice.co_cta_ingr_egr, invoice.co_ven,
                                                      invoice.co_cond, invoice.fec_emis, invoice.fec_venc, invoice.fec_reg, invoice.anulado, invoice.status, invoice.tasa,
                                                      n_cont, invoice.porc_desc_glob, invoice.monto_desc_glob, invoice.porc_reca, invoice.monto_reca, invoice.saldo, invoice.total_bruto,
                                                      invoice.monto_imp, invoice.monto_imp2, invoice.monto_imp3, invoice.otros1, invoice.otros2, invoice.otros3, invoice.total_neto,
                                                      null, invoice.comentario, invoice.dir_ent, invoice.contrib, invoice.impresa, invoice.salestax, invoice.impfis, invoice.impfisfac,
                                                      invoice.ven_ter, invoice.campo1, invoice.campo2, invoice.campo3, invoice.campo4, invoice.campo5, invoice.campo6, invoice.campo7,
                                                      invoice.campo8, user, sucur, invoice.revisado, invoice.trasnfe, "SERVER PROFIT WEB");

                    foreach (saFacturaVentaReng r in invoice.saFacturaVentaReng)
                    {
                        db.pInsertarRenglonesFacturaVenta(r.reng_num, r.doc_num, r.co_art, r.des_art, r.co_uni, r.sco_uni, r.co_alma, r.co_precio, r.tipo_imp, r.tipo_imp2,
                                                          r.tipo_imp3, r.total_art, r.stotal_art, r.prec_vta, r.porc_desc, r.monto_desc, r.reng_neto, r.pendiente, r.pendiente2,
                                                          r.monto_desc_glob, r.monto_reca_glob, r.otros1_glob, r.otros2_glob, r.otros3_glob, r.monto_imp_afec_glob, r.monto_imp2_afec_glob,
                                                          r.monto_imp3_afec_glob, r.tipo_doc, r.rowguid_doc, r.num_doc, r.porc_imp, r.porc_imp2, r.porc_imp3, r.monto_imp, r.monto_imp2, 
                                                          r.monto_imp3, r.otros, r.total_dev, r.monto_dev, r.comentario, null, sucur, user, r.revisado, r.trasnfe, "SERVER PROFIT WEB");
                    }
                    
                    var enumerator = sp.GetEnumerator();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    newInvoice = null;
                }
            }

            return newInvoice;
        }
    }
}