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
            saFacturaVenta invoice = new saFacturaVenta();

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
            }

            return invoice;
        }

        public List<saFacturaVenta> GetAllSaleInvoices(int number, string sucur)
        {
            List<saFacturaVenta> invoices = new List<saFacturaVenta>();

            try
            {
                invoices = db.saFacturaVenta.AsNoTracking().Where(i => i.co_sucu_in == sucur).Include("saFacturaVentaReng").Include("saCliente")
                    .Include("saVendedor").Include("saCondicionPago").OrderByDescending(i => i.fe_us_in).ThenBy(i => i.doc_num).Take(number).ToList();

                foreach (saFacturaVenta invoice in invoices)
                {
                    invoice.saCliente.saFacturaVenta = null;
                    invoice.saVendedor.saFacturaVenta = null;
                    invoice.saCondicionPago.saFacturaVenta = null;
                    foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
                    {
                        reng.saFacturaVenta = null;
                    }
                }
            }
            catch (Exception ex)
            {
                invoices = null;
            }

            return invoices;
        }

        public static saDocumentoVenta GetDocFromSaleInvoice(string id)
        {
            saDocumentoVenta doc = new saDocumentoVenta();

            try
            {
                doc = db.saDocumentoVenta.AsNoTracking().Single(i => i.co_tipo_doc.Trim() == "FACT" && i.nro_doc == id);
            }
            catch (Exception ex)
            {
                doc = null;
            }

            return doc;
        }

        public static saFacturaCompra GetBuyInvoiceByID(string id)
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

        public static saDocumentoCompra GetDocFromBuyInvoice(string id)
        {
            saDocumentoCompra doc = new saDocumentoCompra();

            try
            {
                doc = db.saDocumentoCompra.AsNoTracking().Single(i => i.co_tipo_doc.Trim() == "FACT" && i.nro_doc == id);
            }
            catch (Exception ex)
            {
                doc = null;
            }

            return doc;
        }

        public object GetStatsInvoices(DateTime fec_d, DateTime fec_h)
        {
            int totalCount = 0;
            decimal totalAmountSale = 0, totalAmountPurchase = 0, totalState;

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
            saFacturaVenta new_invoice = new saFacturaVenta();

            using (DbContextTransaction tran = new ProfitAdmEntities(entity.ToString()).Database.BeginTransaction())
            {
                try
                {
                    string n_fact = "", n_cont = "";
                    foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
                    {
                        db.pStockPendienteActualizar(reng.rowguid_doc, reng.total_art, "PCLI");
                    }

                    var sp_n_fact = db.pConsecutivoProximo(sucur, "DOC_VEN_FACT").GetEnumerator();
                    if (sp_n_fact.MoveNext())
                        n_fact = sp_n_fact.Current;

                    sp_n_fact.Dispose();


                    if (string.IsNullOrEmpty(invoice.n_control))
                    {
                        IEnumerator<string> sp_n_cont;
                        
                        do
                        {
                            sp_n_cont = db.pConsecutivoProximo(sucur, "FACT_VTA_N_CON").GetEnumerator();
                            
                            if (sp_n_cont.MoveNext())
                                n_cont = sp_n_cont.Current;

                            sp_n_cont.Dispose();

                        } while (db.saFacturaVenta.AsNoTracking().SingleOrDefault(i => i.n_control.Trim() == n_cont) != null);

                        sp_n_cont.Dispose();
                    }
                    else
                    {
                        n_cont = invoice.n_control;
                    }

                    // FACTURA
                    var sp = db.pInsertarFacturaVenta(n_fact, invoice.descrip, invoice.co_cli, invoice.co_tran, invoice.co_mone, invoice.co_cta_ingr_egr, invoice.co_ven,
                        invoice.co_cond, invoice.fec_emis, invoice.fec_venc, invoice.fec_reg, invoice.anulado, invoice.status, invoice.tasa, n_cont, invoice.porc_desc_glob, 
                        invoice.monto_desc_glob, invoice.porc_reca, invoice.monto_reca, invoice.saldo, invoice.total_bruto, invoice.monto_imp, invoice.monto_imp2, 
                        invoice.monto_imp3, invoice.otros1, invoice.otros2, invoice.otros3, invoice.total_neto, null, invoice.comentario, invoice.dir_ent, invoice.contrib, 
                        invoice.impresa, invoice.salestax, invoice.impfis, invoice.impfisfac, invoice.ven_ter, invoice.campo1, invoice.campo2, invoice.campo3, 
                        invoice.campo4, invoice.campo5, invoice.campo6, invoice.campo7, invoice.campo8, user, sucur, invoice.revisado, invoice.trasnfe, "SERVER PROFIT WEB");

                    // RENGLONES
                    foreach (saFacturaVentaReng r in invoice.saFacturaVentaReng)
                    {
                        db.pInsertarRenglonesFacturaVenta(r.reng_num, n_fact, r.co_art, r.des_art, r.co_uni, r.sco_uni, r.co_alma, r.co_precio, r.tipo_imp, r.tipo_imp2,
                            r.tipo_imp3, r.total_art, r.stotal_art, r.prec_vta, r.porc_desc, r.monto_desc, r.reng_neto, r.pendiente, r.pendiente2, r.monto_desc_glob, 
                            r.monto_reca_glob, r.otros1_glob, r.otros2_glob, r.otros3_glob, r.monto_imp_afec_glob, r.monto_imp2_afec_glob, r.monto_imp3_afec_glob, 
                            r.tipo_doc, r.rowguid_doc, r.num_doc, r.porc_imp, r.porc_imp2, r.porc_imp3, r.monto_imp, r.monto_imp2, r.monto_imp3, r.otros, r.total_dev, 
                            r.monto_dev, r.comentario, null, sucur, user, r.revisado, r.trasnfe, "SERVER PROFIT WEB");
                    }

                    // DOCUMENTO VENTA
                    var sp_doc = db.pInsertarDocumentoVenta("FACT", n_fact, invoice.co_cli, invoice.co_ven, invoice.co_mone, null, invoice.co_cta_ingr_egr, invoice.tasa,
                        string.Format("FACT N° {0} de Cliente {1}", n_fact, invoice.co_cli), invoice.fec_reg, invoice.fec_emis, invoice.fec_venc, invoice.anulado, true, invoice.contrib,
                        "FACT", n_fact, null, invoice.monto_imp, invoice.saldo, invoice.total_bruto, invoice.monto_desc_glob, invoice.porc_desc_glob, invoice.porc_reca, 
                        invoice.monto_reca, invoice.total_neto, invoice.monto_imp2, invoice.monto_imp3, null, 0, 0, 0, 0, null, n_cont, null, 0, 0, 0, 0, 0, 0, 
                        0, invoice.salestax, invoice.ven_ter, invoice.impfis, invoice.impfisfac, invoice.imp_nro_z, invoice.otros1, invoice.otros2, invoice.otros3, invoice.campo1,
                        invoice.campo2, invoice.campo3, invoice.campo4, invoice.campo5, invoice.campo6, invoice.campo7, invoice.campo8, invoice.revisado, invoice.trasnfe, sucur, user,
                        "SERVER PROFIT WEB");

                    new_invoice = GetSaleInvoiceByID(n_fact);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    new_invoice.descrip = "ERROR";
                    new_invoice.comentario = ex.Message;
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

            saPista pista_i = new saPista();
            pista_i.fecha = DateTime.Now;
            pista_i.tablaOri = "saFacturaVenta";
            pista_i.rowguidOri = invoice.rowguid;
            pista_i.usuario_id = user;
            pista_i.tipo_op = "M";
            pista_i.campos = "ANULACION - " + id;
            pista_i.rowguid = Guid.NewGuid();

            saPista pista_d = new saPista();
            pista_d.fecha = DateTime.Now;
            pista_d.tablaOri = "saDocumentoVenta";
            pista_d.rowguidOri = doc.rowguid;
            pista_d.usuario_id = user;
            pista_d.tipo_op = "M";
            pista_d.campos = "ANULACION - " + id;
            pista_d.rowguid = Guid.NewGuid();

            db.Entry(invoice).State = EntityState.Modified;
            db.Entry(doc).State = EntityState.Modified;
            db.saPista.Add(pista_i);
            db.saPista.Add(pista_d);

            db.SaveChanges();
        }
    }
}