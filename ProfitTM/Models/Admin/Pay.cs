using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProfitTM.Models
{
    public class Pay : ProfitAdmManager
    {
        public static saPago GetPayByID(string id)
        {
            saPago pay;

            try
            {
                pay = db.saPago.AsNoTracking().Single(c => c.cob_num == id);
            }
            catch (Exception ex)
            {
                pay = null;
                Incident.CreateIncident("ERROR BUSCANDO PAGO " + id, ex);
            }

            return pay;
        }

        public List<saPago> GetAllPays(int number, string sucur)
        {
            List<saPago> pays;

            try
            {
                pays = db.saPago.AsNoTracking().Where(p => p.co_sucu_in == sucur).Include("saPagoDocReng").Include("saPagoTPReng").Include("saProveedor")
                    .Include("saMoneda").OrderByDescending(c => c.fe_us_in).ThenBy(p => p.cob_num).Take(number).ToList();

                foreach (saPago pay in pays)
                {
                    pay.saProveedor.saPago = null;
                    pay.saMoneda.saCobro = null;
                    foreach (saPagoDocReng reng in pay.saPagoDocReng)
                    {
                        reng.saPago = null;
                        reng.saDocumentoCompra = db.saDocumentoCompra.AsNoTracking().Single(d => d.co_tipo_doc == reng.co_tipo_doc && d.nro_doc == reng.nro_doc);
                        reng.saDocumentoCompra.saTipoDocumento = db.saTipoDocumento.AsNoTracking().Single(t => t.co_tipo_doc == reng.co_tipo_doc);
                    }
                    foreach (saPagoTPReng reng in pay.saPagoTPReng)
                    {
                        reng.saPago = null;
                        reng.saCaja = db.saCaja.AsNoTracking().SingleOrDefault(c => c.cod_caja == reng.cod_caja);
                        reng.saCuentaBancaria = db.saCuentaBancaria.AsNoTracking().SingleOrDefault(c => c.cod_cta == reng.cod_cta);
                    }
                }
            }
            catch (Exception ex)
            {
                pays = null;
                Incident.CreateIncident("ERROR BUSCANDO PAGOS", ex);
            }

            return pays;
        }

        public saPago AddDocAdel(saDocumentoCompra doc, string user, string sucur, int conn)
        {
            saPago pay = new saPago();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        // DATOS
                        decimal mont_doc_usd = doc.total_neto;
                        decimal mont_doc_bsd = Math.Round(doc.total_neto * doc.tasa, 2);
                        string cod_caja = doc.campo1.ToUpper();
                        string co_cta_ingr_egr = context.saCaja.AsNoTracking().Single(b => b.cod_caja == cod_caja).campo1;

                        // SERIE PAGO 
                        string n_pay = GetNextConsec(context, sucur, "PAGO");

                        // ACTUALIZAR SALDO CAJA
                        var sp_s = context.pSaldoActualizar(cod_caja, "EF", "EF", mont_doc_usd, false, "PAGO", false);
                        sp_s.Dispose();

                        // SERIE MOVIMIENTO CAJA
                        string n_mov_c = GetNextConsec(context, sucur, "MOVC_NUM");

                        // INSERTAR MOVIMIENTO CAJA
                        var sp_m = context.pInsertarMovimientoCaja(n_mov_c, DateTime.Now, doc.observa.ToUpper(), cod_caja, doc.tasa, "E", "EF", null, null, null, null, 
                            co_cta_ingr_egr, mont_doc_usd, false, "PAG", n_pay, null, false, false, false, false, null, DateTime.Now, null, null, null, null, null, null, 
                            null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                        sp_m.Dispose();

                        // AGREGAR RETIRO A CAJA
                        BoxMoves move = Box.AddMove(user, cod_caja, mont_doc_usd, false, string.Format("{0} (PAG. {1})", doc.observa.ToUpper(), n_pay.Trim()), conn);

                        // SERIE ADELANTO
                        string n_adel = GetNextConsec(context, sucur, "DOC_COM_ADEL");

                        // INSERTAR ADELANTO
                        var sp_a = context.pInsertarDocumentoCompra("ADEL", n_adel, null, "US$", doc.co_prov, null, "PAGO", null, n_pay, null, null, null, false, true, 0, 
                            doc.observa.ToUpper(), "7", null, null, DateTime.Now, DateTime.Now, DateTime.Now, mont_doc_bsd, doc.tasa, 0, 0, 0, 0, 0, 0, mont_doc_bsd, 0, 
                            0, mont_doc_bsd, 0, 0, 0, 0, null, null, null, 0, 0, null, null, null, null, null, null, null, null, null, null, null, null, null, sucur, user, 
                            "SERVER PROFIT WEB", false);
                        sp_a.Dispose();

                        // INSERTAR PAGO ADELANTO
                        var sp_p = context.pInsertarPago(n_pay, null, doc.co_prov, "US$", doc.tasa, DateTime.Now, false, doc.total_neto, null, doc.observa.ToUpper(), null, 
                            null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);
                        sp_p.Dispose();

                        // INSERTAR DOC PAGO ADELANTO
                        var sp_d = context.pInsertarRenglonesDocPago(1, n_pay, "ADEL", n_adel, null, 0, 0, 0, 0, 0, null, null, null, null, Guid.NewGuid(), null, null, sucur,
                            user, null, null, "SERVER PROFIT WEB");
                        sp_d.Dispose();

                        // INSERTAR TP PAGO ADELANTO
                        var sp_t = context.pInsertarRenglonesTPPago(1, n_pay, "EF", n_mov_c, null, null, false, mont_doc_bsd, null, cod_caja, DateTime.Now, sucur, user, 
                            null, null, "SERVER PROFIT WEB");
                        sp_t.Dispose();

                        context.SaveChanges();
                        tran.Commit();

                        pay = GetPayByID(n_pay);
                        pay.campo1 = move.BoxID.ToString();
                        pay.campo2 = move.ID.ToString();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO ADELANTO A PROVEEDOR", ex);

                        throw ex;
                    }
                }
            }

            return pay;
        }
    }
}