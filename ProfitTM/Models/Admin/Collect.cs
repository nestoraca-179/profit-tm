using System;
using System.Linq;
using System.Data.Entity;

namespace ProfitTM.Models
{
    public class Collect : ProfitAdmManager
    {
        public static saCobro GetCollectByID(string id)
        {
            saCobro invoice;

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
                        
                        #region CODIGO NUEVO
                        /*
                        saDocumentoVenta doc_v = context.saDocumentoVenta.AsNoTracking().FirstOrDefault(d => d.co_tipo_doc == "FACT" && d.nro_doc == doc_num);

                        decimal igtf = (amount * fact.tasa) * Convert.ToDecimal(0.03);
                        string n_ajpm = "", n_coll = "", n_move = "", dis_cen = "<InformacionContable><Carpeta01><CuentaContable>7.1.01.01.001</CuentaContable></Carpeta01></InformacionContable>";

                        // SERIE COBRO
                        var sp_n_coll = context.pConsecutivoProximo(sucur, "COBRO").GetEnumerator();
                        if (sp_n_coll.MoveNext())
                            n_coll = sp_n_coll.Current;

                        sp_n_coll.Dispose();

                        // ACTUALIZAR SALDO
                        var sp_s = context.pSaldoActualizar("004", "EF", "EF", (amount + igtf) * fact.tasa, true, "COBRO", false);
                        sp_s.Dispose();

                        // SERIE AJPM
                        var sp_n_ajpm = context.pConsecutivoProximo(sucur, "DOC_VEN_AJPM").GetEnumerator();
                        if (sp_n_ajpm.MoveNext())
                            n_ajpm = sp_n_ajpm.Current;

                        sp_n_ajpm.Dispose();

                        // INSERTAR AJPM
                        var sp_i = context.pInsertarDocumentoVenta("AJPM", n_ajpm, fact.co_cli, fact.co_ven, fact.co_mone, null, null, fact.tasa, "RECARGO IGTF FACT N° " + fact.doc_num,
                            DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, null, null, null, 0, 0, igtf, 0, null, null, 0, igtf, 0, 0, "7", 0, 0, 0, 
                            0, null, null, dis_cen, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null, null, null, null, null, null, null, null, 
                            sucur, user, "SERVER PROFIT WEB");

                        // SERIE MOVIMIENTO CAJA
                        var sp_n_move = context.pConsecutivoProximo(sucur, "MOVC_NUM").GetEnumerator();
                        if (sp_n_move.MoveNext())
                            n_move = sp_n_move.Current;

                        sp_n_move.Dispose();

                        // INSERTAR MOVIMIENTO CAJA
                        var sp_m = context.pInsertarMovimientoCaja(n_move, DateTime.Now, "MOVIMIENTO COBRO " + n_coll, "004", fact.tasa, "I", "EF", null, null,
                            null, null, "110301001", amount, false, "COBRO", n_coll, null, false, false, false, false, null, DateTime.Now, null, null, null, null,
                            null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        // INSERTAR COBRO
                        var sp_c = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, amount, null,
                            "COBRO EN DOLARES FACT " + doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        // INSERTAR DOC COBRO
                        var sp_d1 = context.pInsertarRenglonesDocCobro(1, n_coll, "AJPM", n_ajpm, igtf, 0, 0, 0, 0, null, null, null, null,
                            Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                        var sp_d2 = context.pInsertarRenglonesDocCobro(2, n_coll, "FACT", fact.doc_num, (amount * fact.tasa), 0, 0, 0, 0, null, null, null, null, 
                            Guid.NewGuid(), null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                        // INSERTAR TP COBRO
                        var sp_t = context.pInsertarRenglonesTPCobro(1, n_coll, "EF", n_move, null, null, false, (amount * fact.tasa) + igtf, null, null, null, null, "004", 
                            DateTime.Now, sucur, user, null, null, "SERVER PROFIT WEB");

                        // ACTUALIZAR FACTURA
                        fact.saldo -= Math.Round(amount * fact.tasa, 2);
                        fact.status = fact.saldo > 0 ? "1" : "2";
                        context.Entry(fact).State = EntityState.Modified;

                        // ACTUALIZAR DOCUMENTO
                        doc_v.saldo -= Math.Round(amount * fact.tasa, 2);
                        context.Entry(doc_v).State = EntityState.Modified;

                        sp_i.Dispose();
                        sp_m.Dispose();
                        sp_c.Dispose();
                        sp_d1.Dispose();
                        sp_d2.Dispose();
                        sp_t.Dispose();

                        tran.Commit();
                        context.SaveChanges();
                        new_collect = GetCollectByID(n_coll);
                        */
                        #endregion

                        #region CODIGO ANTERIOR
                        string n_coll = "", n_move = "", n_adel = "";

                        // SERIE COBRO
                        var sp_n_coll = context.pConsecutivoProximo(sucur, "COBRO").GetEnumerator();
                        if (sp_n_coll.MoveNext())
                            n_coll = sp_n_coll.Current;

                        sp_n_coll.Dispose();

                        // ACTUALIZAR SALDO
                        // var sp_s = context.pSaldoActualizar(user.ToUpper(), "EF", "EF", amount * fact.tasa, true, "COBRO", false); // BOLIVARES
                        var sp_s = context.pSaldoActualizar(user.ToUpper(), "EF", "EF", amount, true, "COBRO", false); // DOLARES
                        sp_s.Dispose();

                        // SERIE ADELANTO
                        var sp_n_adel = context.pConsecutivoProximo(sucur, "DOC_VEN_ADEL").GetEnumerator();
                        if (sp_n_adel.MoveNext())
                            n_adel = sp_n_adel.Current;

                        sp_n_adel.Dispose();

                        // INSERTAR DOCUMENTO
                        var sp_a = context.pInsertarDocumentoVenta("ADEL", n_adel, fact.co_cli, fact.co_ven, fact.co_mone, null, null, fact.tasa, "COBRO N° " + n_coll,
                            DateTime.Now, DateTime.Now, DateTime.Now, false, true, false, "COBRO", n_coll, null, 0, amount * fact.tasa, amount * fact.tasa, 0, null, null,
                            0, amount * fact.tasa, 0, 0, "7", 0, 0, 0, 0, null, null, null, 0, 0, 0, 0, 0, 0, 0, null, false, null, null, null, 0, 0, 0, null, null, null,
                            null, null, null, null, null, null, null, sucur, user, "SERVER PROFIT WEB");

                        // ACTUALIZAR FACTURA
                        // fact.saldo -= (amount * fact.tasa);
                        fact.status = "1";
                        context.Entry(fact).State = EntityState.Modified;

                        // ACTUALIZAR DOCUMENTO
                        /*doc_v.saldo -= (amount * fact.tasa);
                        context.Entry(doc_v).State = EntityState.Modified;*/

                        // SERIE MOVIMIENTO
                        var sp_n_move = context.pConsecutivoProximo(sucur, "MOVC_NUM").GetEnumerator();
                        if (sp_n_move.MoveNext())
                            n_move = sp_n_move.Current;

                        sp_n_move.Dispose();

                        // INSERTAR MOVIMIENTO
                        var sp_m = context.pInsertarMovimientoCaja(n_move, DateTime.Now, "MOVIMIENTO COBRO " + n_coll, user.ToUpper(), fact.tasa, "I", "EF", null, null,
                            null, null, "110301001", amount, false, "COBRO", n_coll, null, false, false, false, false, null, DateTime.Now, null, null, null, null,
                            null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        // INSERTAR COBRO
                        var sp_c = context.pInsertarCobro(n_coll, null, fact.co_cli, fact.co_ven, fact.co_mone, fact.tasa, DateTime.Now, false, amount, null,
                            "COBRO EN DOLARES FACT " + doc_num, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        // INSERTAR DOC COBRO
                        var sp_d = context.pInsertarRenglonesDocCobro(1, n_coll, "ADEL", n_adel, 0, 0, 0, 0, 0, null, null, null, null, Guid.NewGuid(),
                            null, null, sucur, user, null, null, "SERVER PROFIT WEB");

                        // INSERTAR TP COBRO
                        var sp_t = context.pInsertarRenglonesTPCobro(1, n_coll, "EF", n_move, null, null, false, amount * fact.tasa, null, null, null, null, user.ToUpper(), DateTime.Now,
                            sucur, user, null, null, "SERVER PROFIT WEB");

                        sp_a.Dispose();
                        sp_m.Dispose();
                        sp_c.Dispose();
                        sp_d.Dispose();
                        sp_t.Dispose();

                        Box.AddSale(doc_num, amount, user);

                        tran.Commit();
                        context.SaveChanges();
                        new_collect = GetCollectByID(n_coll);
                        #endregion
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