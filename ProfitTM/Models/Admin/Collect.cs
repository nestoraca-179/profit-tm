using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Collect : ProfitAdmManager
    {
        public static saCobro GetCollectByID(string id)
        {
            saCobro invoice = new saCobro();

            try
            {
                invoice = db.saCobro.AsNoTracking().Single(c => c.cob_num == id);
            }
            catch (Exception ex)
            {
                invoice = null;
                Incident.CreateIncident("ERROR BUSCANDO COBRO " + id, ex);
            }

            return invoice;
        }

        public saCobro AddCollectFromInvoice(string doc_num, decimal amount, string user, string sucur)
        {
            saCobro new_collect = new saCobro();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        saFacturaVenta fact = context.saFacturaVenta.AsNoTracking().FirstOrDefault(f => f.doc_num == doc_num);
                        saDocumentoVenta doc_v = context.saDocumentoVenta.AsNoTracking().FirstOrDefault(d => d.co_tipo_doc == "FACT" && d.nro_doc == doc_num);

                        string n_coll = "", n_move = "";

                        // SERIE COBRO
                        var sp_n_coll = context.pConsecutivoProximo(sucur, "COBRO").GetEnumerator();
                        if (sp_n_coll.MoveNext())
                            n_coll = sp_n_coll.Current;

                        sp_n_coll.Dispose();

                        // ACTUALIZAR SALDO
                        var sp_s = context.pSaldoActualizar("004", "EF", "EF", amount * fact.tasa, true, "COBRO", false);
                        sp_s.Dispose();

                        // ACTUALIZAR FACTURA
                        fact.saldo -= (amount * fact.tasa);
                        fact.status = fact.saldo > 0 ? "1" : "2";
                        context.Entry(fact).State = EntityState.Modified;

                        // ACTUALIZAR DOCUMENTO
                        doc_v.saldo -= (amount * fact.tasa);
                        context.Entry(doc_v).State = EntityState.Modified;

                        // SERIE MOVIMIENTO
                        var sp_n_move = context.pConsecutivoProximo(sucur, "MOVC_NUM").GetEnumerator();
                        if (sp_n_move.MoveNext())
                            n_move = sp_n_move.Current;

                        sp_n_move.Dispose();

                        // INSERTAR MOVIMIENTO
                        var sp_m = context.pInsertarMovimientoCaja(n_move, DateTime.Now, "MOVIMIENTO COBRO " + n_coll, "004", fact.tasa, "I", "EF", null, null,
                            null, null, "110301001", amount, false, "COBRO", n_coll, null, false, false, false, false, null, DateTime.Now, null, null, null, null,
                            null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        // INSERTAR COBRO
                        var sp_c = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, amount, null,
                            "COBRO EN DOLARES FACT " + doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        // INSERTAR DOC COBRO
                        var sp_d = context.pInsertarRenglonesDocCobro(1, n_coll, "FACT", doc_num, amount * fact.tasa, 0, 0, 0, 0, null, null, null, null, Guid.NewGuid(), 
                            null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                        // INSERTAR TP COBRO
                        var sp_t = context.pInsertarRenglonesTPCobro(1, n_coll, "EF", n_move, null, null, false, amount * fact.tasa, null, null, null, null, "004", DateTime.Now,
                            sucur, user, null, null, "SERVER PROFIT WEB");

                        sp_m.Dispose();
                        sp_c.Dispose();
                        sp_d.Dispose();
                        sp_t.Dispose();

                        tran.Commit();
                        context.SaveChanges();
                        new_collect = GetCollectByID(n_coll);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        new_collect.descrip = "ERROR";
                        new_collect.campo1 = ex.Message;
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO COBRO DE CONTADO", ex);
                    }
                }
            }
            
            return new_collect;
        }
    }
}