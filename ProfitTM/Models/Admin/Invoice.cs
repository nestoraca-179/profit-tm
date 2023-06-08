using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Invoice : ProfitAdmManager
    {
        public static saFacturaVenta GetSaleInvoiceByID(string id)
        {
            saFacturaVenta invoice;

            try
            {
                invoice = db.saFacturaVenta.AsNoTracking().Include("saFacturaVentaReng").Include("saCliente")
                    .Include("saCondicionPago").Include("saVendedor").Single(i => i.doc_num == id);

                invoice.saCliente.saFacturaVenta = null;
                invoice.saVendedor.saFacturaVenta = null;
                invoice.saCondicionPago.saFacturaVenta = null;
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
                invoices = db.saFacturaVenta.AsNoTracking().Where(i => i.co_sucu_in == sucur).Include("saFacturaVentaReng").Include("saCliente")
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
                invoice.saCondicionPago.saFacturaVenta = null;
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
                    invoice.saCondicionPago.saFacturaVenta = null;
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
                totalCountSale++;
                totalAmountSale += Math.Round(decimal.Parse(enumerator1.Current.total_neto.ToString()), 2);

                if (enumerator1.Current.co_sucu_in.Trim() == sucur)
                {
                    totalCountSaleSuc++;
                    totalAmountSaleSuc += Math.Round(decimal.Parse(enumerator1.Current.total_neto.ToString()), 2);
                }
            }

            // COMPRAS
            var sp2 = db.RepCompraxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null);
            var enumerator2 = sp2.GetEnumerator();

            while (enumerator2.MoveNext())
            {
                totalCountBuy++;
                totalAmountBuy += Math.Round(decimal.Parse(enumerator2.Current.total_neto.ToString()), 2);

                if (enumerator2.Current.co_sucu_in.Trim() == sucur)
                {
                    totalCountBuySuc++;
                    totalAmountBuySuc += Math.Round(decimal.Parse(enumerator2.Current.total_neto.ToString()), 2);
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

        public saFacturaVenta AddInvoice(saFacturaVenta invoice, string user, string sucur, bool fromOrder)
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

                        var sp_n_fact = context.pConsecutivoProximo(sucur, "DOC_VEN_FACT").GetEnumerator();
                        if (sp_n_fact.MoveNext())
                            n_fact = sp_n_fact.Current;

                        sp_n_fact.Dispose();

                        if (string.IsNullOrEmpty(invoice.n_control))
                        {
                            do
                            {
                                string consec = "FACT_VTA_N_CON";

                                if (user == "KKCC" || user == "LMMS" || user == "FJPN")
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

                        // FACTURA
                        var sp = context.pInsertarFacturaVenta(n_fact, invoice.descrip, invoice.co_cli, invoice.co_tran, invoice.co_mone, invoice.co_cta_ingr_egr, invoice.co_ven,
                            invoice.co_cond, invoice.fec_emis, invoice.fec_venc, invoice.fec_reg, invoice.anulado, invoice.status, invoice.tasa, n_cont, invoice.porc_desc_glob,
                            invoice.monto_desc_glob, invoice.porc_reca, invoice.monto_reca, invoice.saldo, invoice.total_bruto, invoice.monto_imp, invoice.monto_imp2,
                            invoice.monto_imp3, invoice.otros1, invoice.otros2, invoice.otros3, invoice.total_neto, null, invoice.comentario, invoice.dir_ent, invoice.contrib,
                            invoice.impresa, invoice.salestax, invoice.impfis, invoice.impfisfac, invoice.ven_ter, invoice.campo1, invoice.campo2, invoice.campo3,
                            invoice.campo4, invoice.campo5, invoice.campo6, invoice.campo7, invoice.campo8, user, sucur, invoice.revisado, invoice.trasnfe, "SERVER PROFIT WEB");

                        // RENGLONES
                        foreach (saFacturaVentaReng r in invoice.saFacturaVentaReng)
                        {
                            var sp_reng = context.pInsertarRenglonesFacturaVenta(r.reng_num, n_fact, r.co_art, r.des_art, r.co_uni, r.sco_uni, r.co_alma, r.co_precio, r.tipo_imp, r.tipo_imp2,
                                r.tipo_imp3, r.total_art, r.stotal_art, r.prec_vta, r.porc_desc, r.monto_desc, r.reng_neto, r.pendiente, r.pendiente2, r.monto_desc_glob,
                                r.monto_reca_glob, r.otros1_glob, r.otros2_glob, r.otros3_glob, r.monto_imp_afec_glob, r.monto_imp2_afec_glob, r.monto_imp3_afec_glob,
                                r.tipo_doc, r.rowguid_doc, r.num_doc, r.porc_imp, r.porc_imp2, r.porc_imp3, r.monto_imp, r.monto_imp2, r.monto_imp3, r.otros, r.total_dev,
                                r.monto_dev, r.comentario, null, sucur, user, r.revisado, r.trasnfe, "SERVER PROFIT WEB");

                            sp_reng.Dispose();
                        }

                        // DOCUMENTO VENTA
                        var sp_doc = context.pInsertarDocumentoVenta("FACT", n_fact, invoice.co_cli, invoice.co_ven, invoice.co_mone, null, invoice.co_cta_ingr_egr, invoice.tasa,
                            string.Format("FACT N° {0} de Cliente {1}", n_fact, invoice.co_cli), invoice.fec_reg, invoice.fec_emis, invoice.fec_venc, invoice.anulado, true, invoice.contrib,
                            "FACT", n_fact, null, invoice.monto_imp, invoice.saldo, invoice.total_bruto, invoice.monto_desc_glob, invoice.porc_desc_glob, invoice.porc_reca,
                            invoice.monto_reca, invoice.total_neto, invoice.monto_imp2, invoice.monto_imp3, null, 0, 0, 0, 0, null, n_cont, null, 0, 0, 0, 0, 0, 0,
                            0, invoice.salestax, invoice.ven_ter, invoice.impfis, invoice.impfisfac, invoice.imp_nro_z, invoice.otros1, invoice.otros2, invoice.otros3, invoice.campo1,
                            invoice.campo2, invoice.campo3, invoice.campo4, invoice.campo5, invoice.campo6, invoice.campo7, invoice.campo8, invoice.revisado, invoice.trasnfe, sucur, user,
                            "SERVER PROFIT WEB");

                        sp.Dispose();
                        sp_doc.Dispose();

                        tran.Commit();
                        new_invoice = GetSaleInvoiceByID(n_fact);
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

        public void SetPrinted(string id)
        {
            saFacturaVenta invoice = GetSaleInvoiceByID(id);
            invoice.impresa = true;

            db.Entry(invoice).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void SetCancelled(string id, string user)
        {
            saFacturaVenta invoice = GetSaleInvoiceByID(id);
            saDocumentoVenta doc = GetDocFromSaleInvoice(id);

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

            var results = db.Database.SqlQuery<string>(string.Format(@"select top 1 (
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

            results = db.Database.SqlQuery<string>(string.Format(@"select top 1 (
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

            return result;
        }
    }
}