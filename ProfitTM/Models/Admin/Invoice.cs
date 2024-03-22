using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using ProfitTM.Controllers;
using System.Data.Entity.Core.EntityClient;

namespace ProfitTM.Models
{
    public class Invoice : ProfitAdmManager
    {
        public saFacturaVenta GetSaleInvoiceByID(string id)
        {
            saFacturaVenta invoice;

            try
            {
                invoice = db.saFacturaVenta.AsNoTracking().Include("saFacturaVentaReng").Include("saCliente")
                    .Include("saCondicionPago").Include("saVendedor").Single(i => i.doc_num == id);

                List<bool> rets = HasRet(invoice.doc_num);

                invoice.saCliente.saFacturaVenta = null;
                invoice.saVendedor.saFacturaVenta = null;
                invoice.saCondicionPago.saFacturaVenta = null;
                invoice.co_us_in = rets[0].ToString();
                invoice.co_us_mo = rets[1].ToString();
                foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
                {
                    reng.saFacturaVenta = null;
                }
            }
            catch (Exception ex)
            {
                invoice = null;
                Incident.CreateIncident("ERROR BUSCANDO FACTURA DE VENTA " + id, ex);
            }

            return invoice;
        }

        public List<saFacturaVenta> GetAllSaleInvoices(int number, string sucur)
        {
            List<saFacturaVenta> invoices;

            try
            {
                using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
                {
                    invoices = context.saFacturaVenta.AsNoTracking().Where(i => i.co_sucu_in == sucur).Include("saFacturaVentaReng").Include("saCliente")
                        .Include("saVendedor").Include("saCondicionPago").OrderByDescending(i => i.fe_us_in).ThenBy(i => i.doc_num).Take(number).ToList();

                    foreach (saFacturaVenta invoice in invoices)
                    {
                        List<bool> rets = HasRet(invoice.doc_num);

                        invoice.saCliente.saFacturaVenta = null;
                        invoice.saVendedor.saFacturaVenta = null;
                        invoice.saCondicionPago.saFacturaVenta = null;
                        invoice.co_us_in = rets[0].ToString();
                        invoice.co_us_mo = rets[1].ToString();
                        foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
                        {
                            reng.saFacturaVenta = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                invoices = null;
                Incident.CreateIncident("ERROR BUSCANDO FACTURAS DE VENTA", ex);
            }

            return invoices;
        }

        public static saDocumentoVenta GetDocFromSaleInvoice(string id)
        {
            saDocumentoVenta doc;

            try
            {
                doc = db.saDocumentoVenta.AsNoTracking().Single(i => i.co_tipo_doc.Trim() == "FACT" && i.nro_doc == id);
            }
            catch (Exception ex)
            {
                doc = null;
                Incident.CreateIncident("ERROR DOCUMENTO DE VENTA DE FACTURA " + id, ex);
            }

            return doc;
        }

        public static saFacturaCompra GetBuyInvoiceByID(string id)
        {
            saFacturaCompra invoice;

            try
            {
                invoice = db.saFacturaCompra.AsNoTracking().Include("saFacturaCompraReng").Include("saProveedor")
                    .Include("saCondicionPago").Single(i => i.doc_num == id);

                invoice.saProveedor.saFacturaCompra = null;
                invoice.saCondicionPago.saFacturaCompra = null;
                foreach (saFacturaCompraReng reng in invoice.saFacturaCompraReng)
                {
                    reng.saFacturaCompra = null;
                }
            }
            catch (Exception ex)
            {
                invoice = null;
                Incident.CreateIncident("ERROR BUSCANDO FACTURA DE COMPRA " + id, ex);
            }

            return invoice;
        }

        public List<saFacturaCompra> GetAllBuyInvoices(int number, string sucur)
        {
            List<saFacturaCompra> invoices;

            try
            {
                invoices = db.saFacturaCompra.AsNoTracking().Where(i => i.co_sucu_in == sucur).Include("saFacturaCompraReng").Include("saProveedor")
                    .Include("saCondicionPago").OrderByDescending(i => i.fe_us_in).ThenBy(i => i.doc_num).Take(number).ToList();

                foreach (saFacturaCompra invoice in invoices)
                {
                    invoice.saProveedor.saFacturaCompra = null;
                    invoice.saCondicionPago.saFacturaCompra = null;
                    foreach (saFacturaCompraReng reng in invoice.saFacturaCompraReng)
                    {
                        reng.saFacturaCompra = null;
                    }
                }
            }
            catch (Exception ex)
            {
                invoices = null;
                Incident.CreateIncident("ERROR BUSCANDO FACTURAS DE COMPRA", ex);
            }

            return invoices;
        }

        public static saDocumentoCompra GetDocFromBuyInvoice(string id)
        {
            saDocumentoCompra doc;

            try
            {
                doc = db.saDocumentoCompra.AsNoTracking().Single(i => i.co_tipo_doc.Trim() == "FACT" && i.nro_doc == id);
            }
            catch (Exception ex)
            {
                doc = null;
                Incident.CreateIncident("ERROR DOCUMENTO DE COMPRA DE FACTURA " + id, ex);
            }

            return doc;
        }

        public saPlantillaVenta GetTemplate()
        {
            saPlantillaVenta template;

            try
            {
                template = db.saPlantillaVenta.AsNoTracking().Include("saPlantillaVentaReng").Include("saCliente")
                    .Include("saCondicionPago").Include("saVendedor").Single(i => i.doc_num == "03000000001");

                template.saCliente.saPlantillaVenta = null;
                template.saVendedor.saPlantillaVenta = null;
                template.saCondicionPago.saPlantillaVenta = null;
                foreach (saPlantillaVentaReng reng in template.saPlantillaVentaReng)
                {
                    reng.saPlantillaVenta = null;
                }
            }
            catch (Exception ex)
            {
                template = null;
                Incident.CreateIncident("ERROR BUSCANDO PLANTILLA DE VENTA", ex);
            }

            return template;
        }
        
        public object GetStatsInvoices(DateTime fec_d, DateTime fec_h, string sucur)
        {
            int totalCountSale = 0, totalCountBuy = 0, totalCountSaleSuc = 0, totalCountBuySuc = 0;
            decimal totalAmountSale = 0, totalAmountBuy = 0, totalState;
            decimal totalAmountSaleSuc = 0, totalAmountBuySuc = 0, totalStateSuc;
            decimal totalReimbExpSale = 0, totalReimbExpSaleSuc = 0, totalReimbExpBuy = 0, totalReimbExpBuySuc = 0;

            // VENTAS
            var sp1 = db.RepFacturaVentaxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            var enumerator1 = sp1.GetEnumerator();

            while (enumerator1.MoveNext())
            {
                decimal total_neto_v = enumerator1.Current.anulado ? 0 : enumerator1.Current.total_neto.Value;

                totalCountSale++;
                totalAmountSale += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);

                if (enumerator1.Current.co_sucu_in?.Trim() == sucur)
                {
                    totalCountSaleSuc++;
                    totalAmountSaleSuc += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);
                }
            }

            // COMPRAS
            var sp2 = db.RepCompraxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null);
            var enumerator2 = sp2.GetEnumerator();

            while (enumerator2.MoveNext())
            {
                decimal total_neto_c = enumerator2.Current.anulado ? 0 : enumerator2.Current.total_neto.Value;

                totalCountBuy++;
                totalAmountBuy += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);

                if (enumerator2.Current.co_sucu_in?.Trim() == sucur)
                {
                    totalCountBuySuc++;
                    totalAmountBuySuc += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);
                }
            }

            // ESTADO DE GANANCIA
            totalState = totalAmountSale - totalAmountBuy;
            totalStateSuc = totalAmountSaleSuc - totalAmountBuySuc;

            // GASTOS REEMBOLSABLES - ISH
            if (db.Database.Connection.Database == "PP2K12_ISH_ADM")
            {
                // totalReimbExpSale
                var sp3 = db.RepFacturaVentaxArt2("910101001-001", "910101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
                    null, null, null, null, null, null, null, null, null, null);
                var enumerator3 = sp3.GetEnumerator();

                while (enumerator3.MoveNext())
                {
                    if (enumerator3.Current.anulado)
                        totalReimbExpSale += 0;
                    else
                        totalReimbExpSale += Convert.ToDecimal(enumerator3.Current.neto);
                }
                // totalReimbExpSale

                // totalReimbExpSaleSuc
                var sp4 = db.RepFacturaVentaxArt2("910101001-001", "910101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
                    null, null, null, null, null, null, sucur, null, null, null);
                var enumerator4 = sp4.GetEnumerator();

                while (enumerator4.MoveNext())
                {
                    if (enumerator4.Current.anulado)
                        totalReimbExpSaleSuc += 0;
                    else
                        totalReimbExpSaleSuc += Convert.ToDecimal(enumerator4.Current.neto);
                }
                // totalReimbExpSaleSuc

                // totalReimbExpBuy
                var sp5 = db.RepCompraxArt2("920101001-001", "920101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
                    null, null, null, null, null, null);
                var enumerator5 = sp5.GetEnumerator();

                while (enumerator5.MoveNext())
                {
                    if (enumerator5.Current.anulado)
                        totalReimbExpBuy += 0;
                    else
                        totalReimbExpBuy += Convert.ToDecimal(enumerator5.Current.neto);
                }
                // totalReimbExpBuy

                // totalReimbExpBuySuc
                var sp6 = db.RepCompraxArt2("920101001-001", "920101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
                    null, null, sucur, null, null, null);
                var enumerator6 = sp6.GetEnumerator();

                while (enumerator6.MoveNext())
                {
                    if (enumerator6.Current.anulado)
                        totalReimbExpBuySuc += 0;
                    else
                        totalReimbExpBuySuc += Convert.ToDecimal(enumerator6.Current.neto);
                }
                // totalReimbExpBuySuc

                enumerator3.Dispose();
                enumerator4.Dispose();
                enumerator5.Dispose();
                enumerator6.Dispose();
            }

            enumerator1.Dispose();
            enumerator2.Dispose();

            // OBJETO ESTADISTICAS
            var obj = new 
            {
                all = new {
                    totalCountSale,
                    totalCountBuy,
                    totalAmountSale,
                    totalAmountBuy,
                    totalState,
                    totalReimbExpSale,
                    totalReimbExpBuy
                },
                suc = new
                {
                    totalCountSale = totalCountSaleSuc,
                    totalCountBuy = totalCountBuySuc,
                    totalAmountSale = totalAmountSaleSuc,
                    totalAmountBuy = totalAmountBuySuc,
                    totalState = totalStateSuc,
                    totalReimbExpSale = totalReimbExpSaleSuc,
                    totalReimbExpBuy = totalReimbExpBuySuc
                },
            };

            return obj;
        }

        public saFacturaVenta AddSaleInvoice(saFacturaVenta invoice, string user, string sucur, int conn, bool fromOrder)
        {
            saFacturaVenta new_invoice = new saFacturaVenta();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        string n_fact = "", n_cont = "";
                        
                        if (fromOrder)
                        {
                            foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
                            {
                                context.pStockPendienteActualizar(reng.rowguid_doc, reng.total_art, "PCLI");
                            }
                        }

                        if (string.IsNullOrEmpty(invoice.doc_num))
                        {
                            var sp_n_fact = context.pConsecutivoProximo(sucur, "DOC_VEN_FACT").GetEnumerator();
                            if (sp_n_fact.MoveNext())
                                n_fact = sp_n_fact.Current;

                            sp_n_fact.Dispose();
                        }
                        else
                        {
                            n_fact = invoice.doc_num;
                        }

                        if (string.IsNullOrEmpty(invoice.n_control))
                        {
                            do
                            {
                                string consec = "FACT_VTA_N_CON";

                                if (User.GetUserByName(user).UseAlterSerie)
                                    consec = "FACT_VTA_N_CON_2";

                                var sp_n_cont = context.pConsecutivoProximo(sucur, consec).GetEnumerator();
                                if (sp_n_cont.MoveNext())
                                    n_cont = sp_n_cont.Current;

                                sp_n_cont.Dispose();

                            } while (context.saFacturaVenta.AsNoTracking().SingleOrDefault(i => i.n_control.Trim() == n_cont) != null);
                        }
                        else
                        {
                            n_cont = invoice.n_control;
                        }

                        if (string.IsNullOrEmpty(invoice.comentario)) // VALIDACION DE BASE DE IGTF
                            invoice.comentario = "0";

                        // FACTURA
                        var sp = context.pInsertarFacturaVenta(n_fact, invoice.descrip, invoice.co_cli, invoice.co_tran, invoice.co_mone, null, invoice.co_ven,
                            invoice.co_cond, invoice.fec_emis, invoice.fec_venc, invoice.fec_reg, invoice.anulado, invoice.status, invoice.tasa, n_cont, invoice.porc_desc_glob,
                            invoice.monto_desc_glob, invoice.porc_reca, invoice.monto_reca, invoice.saldo, invoice.total_bruto, invoice.monto_imp, invoice.monto_imp2,
                            invoice.monto_imp3, invoice.otros1, invoice.otros2, invoice.otros3, invoice.total_neto, null, invoice.comentario, invoice.dir_ent, invoice.contrib,
                            invoice.impresa, invoice.salestax, invoice.impfis, invoice.impfisfac, invoice.ven_ter, invoice.campo1, invoice.campo2, invoice.campo3,
                            invoice.campo4, invoice.campo5, invoice.campo6, invoice.campo7, invoice.campo8, user, sucur, invoice.revisado, invoice.trasnfe, "SERVER PROFIT WEB");

                        // RENGLONES
                        foreach (saFacturaVentaReng r in invoice.saFacturaVentaReng)
                        {
                            var sp_reng = context.pInsertarRenglonesFacturaVenta(r.reng_num, n_fact, r.co_art, r.des_art, r.co_uni, r.sco_uni, r.co_alma, r.co_precio, r.tipo_imp, 
                                r.tipo_imp2, r.tipo_imp3, r.total_art, r.stotal_art, r.prec_vta, r.porc_desc, r.monto_desc, r.reng_neto, r.pendiente, r.pendiente2, r.monto_desc_glob,
                                r.monto_reca_glob, r.otros1_glob, r.otros2_glob, r.otros3_glob, r.monto_imp_afec_glob, r.monto_imp2_afec_glob, r.monto_imp3_afec_glob,
                                r.tipo_doc, r.rowguid_doc, r.num_doc, r.porc_imp, r.porc_imp2, r.porc_imp3, r.monto_imp, r.monto_imp2, r.monto_imp3, r.otros, r.total_dev,
                                r.monto_dev, r.comentario, null, sucur, user, r.revisado, r.trasnfe, "SERVER PROFIT WEB");

                            sp_reng.Dispose();
                        }

                        // DOCUMENTO VENTA
                        var sp_doc = context.pInsertarDocumentoVenta("FACT", n_fact, invoice.co_cli, invoice.co_ven, invoice.co_mone, null, null, invoice.tasa,
                            string.Format("FACT N° {0} de Cliente {1}", n_fact.Trim(), invoice.co_cli), invoice.fec_reg, invoice.fec_emis, invoice.fec_venc, invoice.anulado, 
                            true, invoice.contrib, "FACT", n_fact, null, invoice.monto_imp, invoice.saldo, invoice.total_bruto, invoice.monto_desc_glob, invoice.porc_desc_glob, 
                            invoice.porc_reca, invoice.monto_reca, invoice.total_neto, invoice.monto_imp2, invoice.monto_imp3, null, 0, 0, 0, 0, null, n_cont, null, 0, 0, 
                            0, 0, 0, 0, 0, invoice.salestax, invoice.ven_ter, invoice.impfis, invoice.impfisfac, invoice.imp_nro_z, invoice.otros1, invoice.otros2, 
                            invoice.otros3, invoice.campo1, invoice.campo2, invoice.campo3, invoice.campo4, invoice.campo5, invoice.campo6, invoice.campo7, invoice.campo8, 
                            invoice.revisado, invoice.trasnfe, sucur, user, "SERVER PROFIT WEB");

                        sp.Dispose();
                        sp_doc.Dispose();

                        tran.Commit();
                        new_invoice = GetSaleInvoiceByID(n_fact);

                        if (Connection.GetConnByID(conn.ToString()).UseFactOnline)
                        {
                            if (!n_fact.StartsWith("D"))
                            {
                                string serie = new Branch().GetBranchByID(sucur).campo2;
                                string json = new Root().GetJsonInvoiceInfo(new_invoice, serie);
                                LogsFact.Add(new_invoice, conn, json, serie);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO FACTURA CON PRELIQUIDACION", ex);

                        throw ex;
                    }
                }
            }

            return new_invoice;
        }

        public saFacturaCompra AddBuyInvoice(saFacturaCompra invoice, string user, string sucur)
        {
            saFacturaCompra new_invoice = new saFacturaCompra();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        string doc_num = "";
                        string dis_cen = "<InformacionContable><Carpeta01><CuentaContable>1.1.90.05.001</CuentaContable></Carpeta01></InformacionContable>";

                        var sp_doc_num = context.pConsecutivoProximo(sucur, "DOC_COM_FACT").GetEnumerator();
                        if (sp_doc_num.MoveNext())
                            doc_num = sp_doc_num.Current;

                        sp_doc_num.Dispose();

                        // FACTURA
                        var sp = context.pInsertarFacturaCompra(doc_num, invoice.nro_fact, invoice.descrip, invoice.co_prov, invoice.co_cta_ingr_egr, invoice.co_mone, 
                            invoice.co_cond, invoice.n_control, "0", invoice.fec_emis, invoice.fec_venc, invoice.fec_reg, false, invoice.status, invoice.tasa, null, 
                            invoice.saldo, invoice.total_bruto, invoice.total_neto, 0, 0, 0, 0, 0, invoice.monto_imp, 0, 0, null, null, false, null, dis_cen, invoice.campo1, 
                            null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", true);

                        // RENGLONES
                        foreach (saFacturaCompraReng r in invoice.saFacturaCompraReng)
                        {
                            var sp_reng = context.pInsertarRenglonesFacturaCompra(r.reng_num, doc_num, r.co_art, r.des_art, r.co_uni, r.sco_uni, r.co_alma, r.tipo_imp,
                                r.tipo_imp2, r.tipo_imp3, r.tipo_doc, r.porc_desc, r.num_doc, r.rowguid_doc, r.reng_neto, r.cost_unit, r.cost_unit_om, r.total_art,
                                r.stotal_art, r.otros, r.porc_imp, r.porc_imp2, r.porc_imp3, r.monto_imp, r.monto_imp2, r.monto_imp3, r.porc_gas, r.total_dev, r.monto_dev,
                                r.pendiente2, r.comentario, false, r.monto_desc_glob, r.monto_reca_glob, r.otros1_glob, r.otros2_glob, r.otros3_glob, r.monto_imp_afec_glob,
                                r.monto_imp2_afec_glob, r.monto_imp3_afec_glob, r.monto_desc, r.pendiente, null, null, sucur, user, null, null, "SERVER PROFIT WEB", 0, 0, 0,
                                "Totalmente Deducible (Art. 34)");

                            sp_reng.Dispose();
                        }

                        // DOCUMENTO COMPRA
                        var sp_doc = context.pInsertarDocumentoCompra("FACT", doc_num, invoice.nro_fact, invoice.co_mone, invoice.co_prov, invoice.co_cta_ingr_egr, "FACT",
                            null, doc_num, null, null, "0", false, true, 0, string.Format("FACT N° {0} de Proveedor {1}", doc_num.Trim(), invoice.co_prov), "1", null, null, 
                            invoice.fec_reg, invoice.fec_emis, invoice.fec_venc, invoice.total_neto, invoice.tasa, 0, 0, 0, invoice.monto_imp, 0, 0, invoice.total_bruto,
                            0, 0, invoice.saldo, 0, 0, 0, 0, null, null, null, 0, 0, null, null, invoice.n_control, null, null, null, null, null, null, null, null, null,
                            null, sucur, user, "SERVER PROFIT WEB", true);

                        sp.Dispose();
                        sp_doc.Dispose();

                        tran.Commit();
                        new_invoice = GetBuyInvoiceByID(doc_num);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO FACTURA DE COMPRA", ex);

                        throw ex;
                    }
                }
            }

            return new_invoice;
        }
        
        public saDocumentoVenta AddCreditNote(string doc_num, string user, string sucur, int conn)
        {
            saDocumentoVenta new_doc = new saDocumentoVenta();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction()) 
                {
                    try
                    {
                        string n_ncr = "", n_cont = "";
                        string dis_cen = "<InformacionContable><Carpeta01><CuentaContable>1.1.03.01.001</CuentaContable></Carpeta01></InformacionContable>";

                        saFacturaVenta invoice = context.saFacturaVenta.AsNoTracking().Single(i => i.doc_num == doc_num);
                        invoice.saFacturaVentaReng = context.saFacturaVentaReng.AsNoTracking().Where(r => r.doc_num == doc_num).ToList();
                        saDocumentoVenta doc_v = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "FACT" && d.nro_doc == doc_num);

                        n_ncr = GetNextConsec(sucur, "DOC_VEN_N/CR").Trim();
                        n_cont = GetNextConsec(sucur, "N/CR_VTA_N_CON").Trim();

                        // NOTA DE CREDITO
                        var sp = context.pInsertarDocumentoVenta("N/CR", n_ncr, invoice.co_cli, invoice.co_ven, invoice.co_mone, null, null, invoice.tasa, 
                            string.Format("NOTA DE CREDITO DE FACTURA {0}", invoice.doc_num.Trim()), DateTime.Now, DateTime.Now, DateTime.Now, false, false, false, 
                            "FACT", invoice.doc_num, null, invoice.monto_imp, 0, invoice.total_bruto, 0, "0", "0", 0, invoice.total_neto, 0, 0, "1", 0, 16, 0, 
                            0, null, n_cont, dis_cen, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, invoice.campo2, invoice.campo7, invoice.campo3, 
                            invoice.campo8, null, null, null, null, null, null, sucur, user, "SERVER PROFIT WEB");
                        sp.Dispose();

                        doc_v.saldo = 0;
                        context.Entry(doc_v).State = EntityState.Modified;

                        invoice.saldo = 0;
                        context.Entry(invoice).State = EntityState.Modified;

                        // COBRO CRUCE
                        string n_coll = GetNextConsec(sucur, "COBRO");

                        var sp_c = context.pInsertarCobro(n_coll, null, invoice.co_cli, invoice.co_ven, invoice.co_mone, invoice.tasa, DateTime.Now, false, 0, 
                            null, string.Format("CRUCE FACT {0} / NCR {1}", doc_num.Trim(), n_ncr.Trim()), null, null, null, null, null, null, null, null, user, 
                            sucur, "SERVER PROFIT WEB", null, null);
                        sp_c.Dispose();

                        var sp_cd1 = context.pInsertarRenglonesDocCobro(1, n_coll, "FACT", doc_num, invoice.total_neto, 0, 0, 0, 0, null, null, null, null, Guid.NewGuid(), 
                            null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                        sp_cd1.Dispose();

                        var sp_cd2 = context.pInsertarRenglonesDocCobro(2, n_coll, "N/CR", n_ncr, invoice.total_neto, 0, 0, 0, 0, null, null, null, null, Guid.NewGuid(),
                            null, null, sucur, user, null, null, "SERVER PROFIT WEB");
                        sp_cd2.Dispose();

                        var sp_ct = context.pInsertarRenglonesTPCobro(1, n_coll, "EF", null, null, null, false, 0, null, null, null, null, "001", DateTime.Now, sucur, 
                            user, null, null, "SERVER PROFIT WEB");
                        sp_ct.Dispose();

                        context.SaveChanges();
                        tran.Commit();
                        new_doc = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == "N/CR" && d.nro_doc == n_ncr);

                        if (Connection.GetConnByID(conn.ToString()).UseFactOnline)
                        {
                            string serie = new Branch().GetBranchByID(sucur).campo2;
                            if (string.IsNullOrEmpty(invoice.comentario))
                                invoice.comentario = "0";

                            Root obj = JsonConvert.DeserializeObject<Root>(new Root().GetJsonInvoiceInfo(invoice, serie));

                            obj.documentoElectronico.encabezado.identificacionDocumento.tipoDocumento = "02";
                            obj.documentoElectronico.encabezado.identificacionDocumento.numeroDocumento = n_ncr;
                            obj.documentoElectronico.encabezado.identificacionDocumento.serieFacturaAfectada = serie;
                            obj.documentoElectronico.encabezado.identificacionDocumento.numeroFacturaAfectada = doc_num.Trim();
                            obj.documentoElectronico.encabezado.identificacionDocumento.fechaFacturaAfectada = invoice.fec_emis.ToString("dd/MM/yyyy");
                            obj.documentoElectronico.encabezado.identificacionDocumento.montoFacturaAfectada = invoice.total_neto.ToString().Replace(",", ".");
                            obj.documentoElectronico.encabezado.identificacionDocumento.comentarioFacturaAfectada = "N/CR " + n_ncr + " FACTURA " + doc_num.Trim();
                            obj.documentoElectronico.encabezado.identificacionDocumento.fechaEmision = DateTime.Now.ToString("dd/MM/yyyy");
                            obj.documentoElectronico.encabezado.identificacionDocumento.fechaVencimiento = DateTime.Now.ToString("dd/MM/yyyy");
                            obj.documentoElectronico.encabezado.identificacionDocumento.horaEmision = DateTime.Now.ToString("hh:mm:ss") + (DateTime.Now.Hour < 12 ? " am" : " pm");

                            string json = JsonConvert.SerializeObject(obj);
                            invoice.doc_num = "N-" + n_ncr;
                            LogsFact.Add(invoice, conn, json, serie);
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR AGREGANDO NOTA DE CREDITO DE FACTURA " + doc_num, ex);

                        throw ex;
                    }
                }
            }

            return new_doc;
        }
        
        public void SetPrinted(string id)
        {
            saFacturaVenta invoice = db.saFacturaVenta.AsNoTracking().Single(i => i.doc_num.Trim() == id.Trim());
            invoice.impresa = true;

            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();
        }
        
        public async Task SetCancelledAsync(string id, string user, string serie, string token)
        {
            saFacturaVenta invoice = db.saFacturaVenta.AsNoTracking().Single(i => i.doc_num.Trim() == id.Trim());
            saDocumentoVenta doc = db.saDocumentoVenta.AsNoTracking().Single(i => i.co_tipo_doc == "FACT" && i.nro_doc.Trim() == id.Trim());

            ModelCancelRequest request = new ModelCancelRequest()
            {
                serie = serie,
                tipoDocumento = "01",
                numeroDocumento = id,
                motivoAnulacion = "ANULACION DE FACTURA " + id,
                fechaAnulacion = DateTime.Now.ToString("dd/MM/yyyy"),
                horaAnulacion = DateTime.Now.ToString("hh:mm:ss tt", new CultureInfo("en-US")).ToLower()
            };
            ModelCancelResponse response = await new Root().CancelInvoice(request, token);

            foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
            {
                if (reng.rowguid_doc != null)
                    db.pStockPendienteActualizar(reng.rowguid_doc, reng.total_art * -1, "PCLI");
            }

            invoice.anulado = true;
            invoice.saldo = 0;

            doc.anulado = true;
            doc.saldo = 0;
            doc.observa = doc.observa.Trim() + " | (ANULADO)";

            db.Entry(invoice).State = EntityState.Modified;
            db.Entry(doc).State = EntityState.Modified;

            Step.CreateStep("saFacturaVenta", invoice.rowguid, user, "M", "ANULACION - " + id);
            Step.CreateStep("saDocumentoVenta", doc.rowguid, user, "M", "ANULACION - " + id);

            db.SaveChanges();
        }

        private static List<bool> HasRet(string doc_num)
        {
            List<bool> result = new List<bool>();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                var results = context.Database.SqlQuery<string>(string.Format(@"select top 1 (
                    select CDR2.nro_doc
                    from saCobroDocReng CDR2
                    where CDR2.co_tipo_doc = 'IVAN' and CDR2.rowguid_reng_ori = CDR.rowguid
                ) doc_iva
                from saFacturaVenta FV
                left join saCobroDocReng CDR on CDR.nro_doc = FV.doc_num
                where CDR.cob_num is not null and FV.doc_num = '{0}'
                order by FV.doc_num", doc_num)).ToList();

                if (results.Count > 0)
                    result.Add(results[0] != null);
                else
                    result.Add(false);

                results = context.Database.SqlQuery<string>(string.Format(@"select top 1 (
                    select CDR2.nro_doc
                    from saCobroDocReng CDR2
                    where CDR2.co_tipo_doc = 'ISLR' and CDR2.rowguid_reng_ori = CDR.rowguid
                ) doc_iva
                from saFacturaVenta FV
                left join saCobroDocReng CDR on CDR.nro_doc = FV.doc_num
                where CDR.cob_num is not null and FV.doc_num = '{0}'
                order by FV.doc_num", doc_num)).ToList();

                if (results.Count > 0)
                    result.Add(results[0] != null);
                else
                    result.Add(false);
            }

            return result;
        }

        public static void UpdateControl(LogsFactOnline log, string n_control)
        {
            Connections conn = Connection.GetConnByID(log.ConnID.ToString());
            string n_connect = string.Format("Server={0};Database={1};User Id={2};Password={3}", conn.Server, conn.DB, conn.Username, conn.Password);
            EntityConnectionStringBuilder n_entity = EntityController.GetEntity(n_connect);

            using (ProfitAdmEntities context = new ProfitAdmEntities(n_entity.ToString()))
            {
                bool isFact = !log.NroFact.Contains("N-");
                string tip_doc = "", nro_doc = "";

                if (isFact)
                {
                    saFacturaVenta fact = context.saFacturaVenta.AsNoTracking().Single(i => i.doc_num.Trim() == log.NroFact);
                    fact.n_control = n_control;
                    context.Entry(fact).State = EntityState.Modified;

                    tip_doc = "FACT";
                    nro_doc = log.NroFact;
                }
                else
                {
                    tip_doc = "N/CR";
                    nro_doc = log.NroFact.Replace("N-", "");
                }

                saDocumentoVenta doc = context.saDocumentoVenta.AsNoTracking().Single(d => d.co_tipo_doc == tip_doc && d.nro_doc == nro_doc);
                doc.n_control = n_control;
                context.Entry(doc).State = EntityState.Modified;

                context.SaveChanges();
            }
        }
    }
}