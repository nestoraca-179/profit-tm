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
                move = db.saMovimientoCaja.AsNoTracking().Single(o => o.mov_num == id);
            }
            catch (Exception ex)
            {
                move = null;
                Incident.CreateIncident("ERROR BUSCANDO MOVIMIENTO DE CAJA " + id, ex);
            }

            return move;
        }

        public saMovimientoCaja AddBoxMove(saMovimientoCaja mo, string user, string sucur)
        {
            saMovimientoCaja new_move = new saMovimientoCaja();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        string n_mov = "";
                        var sp_n_mov = context.pConsecutivoProximo(sucur, "MOVC_NUM").GetEnumerator();
                        if (sp_n_mov.MoveNext())
                            n_mov = sp_n_mov.Current;

                        sp_n_mov.Dispose();

                        // ACTUALIZAR SALDO
                        context.pActualizarSaldoCaja(user, user, "EF", "EF", mo.monto_h, Guid.NewGuid());
                        context.pActualizarSaldoCaja(user, user, "TF", "TF", mo.monto_h, Guid.NewGuid());

                        // MOVIMIENTO DE CAJA
                        var sp = context.pInsertarMovimientoCaja(n_mov, DateTime.Now, mo.descrip, user, mo.tasa, mo.tipo_mov, mo.forma_pag, null, null, null, null,
                            mo.co_cta_ingr_egr, mo.monto_h, false, mo.origen, null, null, false, false, false, false, null, DateTime.Now, null, null, null, null,
                            null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB", null, null);

                        sp.Dispose();

                        BoxMoves move = Box.AddIncome(n_mov, mo.monto_h, user, mo.descrip);

                        tran.Commit();
                        new_move = GetBoxMoveByID(n_mov);
                        new_move.campo1 = move.BoxID.ToString();
                        new_move.campo2 = move.ID.ToString();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        new_move.descrip = "ERROR";
                        new_move.campo1 = ex.Message;
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO MOVIMIENTO DE CAJA", ex);
                    }
                }
            }

            return new_move;
        }
    }
}