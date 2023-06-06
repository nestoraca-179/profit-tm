using System;
using System.Linq;
using System.Data.Entity;

namespace ProfitTM.Models
{
    public class BoxMove : ProfitAdmManager
    {
        public static saMovimientoCaja GetBoxMoveByID(string id)
        {
            saMovimientoCaja move;

            try
            {
                move = db.saMovimientoCaja.AsNoTracking().Single(m => m.mov_num == id);
            }
            catch (Exception ex)
            {
                move = null;
                Incident.CreateIncident("ERROR BUSCANDO MOVIMIENTO DE CAJA " + id, ex);
            }

            return move;
        }

        public saMovimientoCaja AddBoxMove(saMovimientoCaja mo, string user, string sucur, bool isIncome, bool newBox, bool transfer, int conn)
        {
            saMovimientoCaja new_move = new saMovimientoCaja();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        string cod_caja = mo.origen == "OPA" ? mo.cod_caja : user, n_mov = "";
                        var sp_n_mov = context.pConsecutivoProximo(sucur, "MOVC_NUM").GetEnumerator();
                        if (sp_n_mov.MoveNext())
                            n_mov = sp_n_mov.Current;

                        sp_n_mov.Dispose();

                        if (newBox) // COLOCAR TASA Y CUENTA A MOV. DE SALDO INICIAL DE CAJA NUEVA
                        {
                            var sp_t = context.pObtenerFechaTasa("US$", DateTime.Now);
                            var enumerator = sp_t.GetEnumerator();

                            while (enumerator.MoveNext())
                                mo.tasa = enumerator.Current.TASA_V.Value;

                            mo.co_cta_ingr_egr = context.saCaja.AsNoTracking().Single(c => c.cod_caja.Trim() == cod_caja).campo1;
                            sp_t.Dispose();
                        }

                        if (transfer) // COLOCAR CUENTA A MOV. DE INGRESO O EGRESO
                        {
                            cod_caja = mo.campo8;

                            if (isIncome)
                                mo.co_cta_ingr_egr = context.saCaja.AsNoTracking().Single(c => c.cod_caja.Trim() == mo.campo8).campo1;
                            else
                                mo.co_cta_ingr_egr = context.saCaja.AsNoTracking().Single(c => c.cod_caja.Trim() == mo.cod_caja.Trim()).campo1;
                        }

                        decimal amount = isIncome ? mo.monto_h : mo.monto_h * -1;

                        // ACTUALIZAR SALDO
                        context.pActualizarSaldoCaja(cod_caja, cod_caja, "EF", "EF", amount, Guid.NewGuid());
                        context.pActualizarSaldoCaja(cod_caja, cod_caja, "TF", "TF", amount, Guid.NewGuid());

                        // MOVIMIENTO DE CAJA
                        var sp = context.pInsertarMovimientoCaja(n_mov, DateTime.Now, mo.descrip, cod_caja, mo.tasa, mo.tipo_mov, mo.forma_pag, null, null, null, null,
                            mo.co_cta_ingr_egr, Math.Abs(amount), false, mo.origen, null, null, false, false, false, false, null, DateTime.Now, null, null, null, null,
                            null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        sp.Dispose();

                        if (transfer) // MOVIMIENTO DE CONTRAPARTE
                        {
                            string n_mov2 = "";
                            var sp_n_mov2 = context.pConsecutivoProximo(sucur, "MOVC_NUM").GetEnumerator();
                            if (sp_n_mov2.MoveNext())
                                n_mov2 = sp_n_mov2.Current;

                            sp_n_mov2.Dispose();

                            decimal amount2 = isIncome ? mo.monto_h * -1 : mo.monto_h;

                            context.pActualizarSaldoCaja(mo.cod_caja, mo.cod_caja, "EF", "EF", amount2, Guid.NewGuid());
                            context.pActualizarSaldoCaja(mo.cod_caja, mo.cod_caja, "TF", "TF", amount2, Guid.NewGuid());

                            mo.campo1 = isIncome ? mo.descrip + " (EGRESO)" : mo.descrip + " (INGRESO)";
                            mo.tipo_mov = isIncome ? "E" : "I";

                            var sp2 = context.pInsertarMovimientoCaja(n_mov2, DateTime.Now, mo.campo1, mo.cod_caja, mo.tasa, mo.tipo_mov, mo.forma_pag, null, null, 
                                null, null, mo.co_cta_ingr_egr, Math.Abs(amount2), false, mo.origen, null, null, false, false, false, false, null, DateTime.Now, null, 
                                null, null, null, null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                            sp2.Dispose();

                            // MOVIMIENTOS PROFIT TM
                            BoxMoves move = Box.AddMove(user, cod_caja, mo.monto_h, isIncome, mo.descrip, conn);
                            BoxMoves move2 = Box.AddMove(user, mo.cod_caja, Math.Abs(isIncome ? mo.monto_h * -1 : mo.monto_h), !isIncome, mo.campo1, conn);

                            new_move = context.saMovimientoCaja.AsNoTracking().Single(m => m.mov_num == n_mov);
                            new_move.campo1 = move.BoxID.ToString();
                            new_move.campo2 = move.ID.ToString();
                            new_move.campo3 = move2.BoxID.ToString();
                            new_move.campo4 = move2.ID.ToString();
                        }
                        else
                        {
                            new_move = context.saMovimientoCaja.AsNoTracking().Single(m => m.mov_num == n_mov);
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO MOVIMIENTO DE CAJA", ex);
                        
                        throw ex;
                    }
                }
            }

            return new_move;
        }
    }
}