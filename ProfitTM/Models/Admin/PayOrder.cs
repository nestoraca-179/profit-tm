using System;
using System.Linq;
using System.Data.Entity;

namespace ProfitTM.Models
{
    public class PayOrder : ProfitAdmManager
    {
        public static saOrdenPago GetPayOrderByID(string id)
        {
            saOrdenPago payOrder = new saOrdenPago();

            try
            {
                payOrder = db.saOrdenPago.AsNoTracking().Single(o => o.ord_num == id);
            }
            catch (Exception ex)
            {
                payOrder = null;
                Incident.CreateIncident("ERROR BUSCANDO COBRO " + id, ex);
            }

            return payOrder;
        }

        public saOrdenPago AddPayOrder(saOrdenPago po, string user, string sucur)
        {
            saOrdenPago new_order = new saOrdenPago();

            using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
            {
                using (DbContextTransaction tran = context.Database.BeginTransaction())
                {
                    try
                    {
                        string n_ord = "";
                        saOrdenPagoReng reng = po.saOrdenPagoReng.ToList()[0];

                        var sp_n_ord = context.pConsecutivoProximo(sucur, "ORDP_NUM").GetEnumerator();
                        if (sp_n_ord.MoveNext())
                            n_ord = sp_n_ord.Current;

                        sp_n_ord.Dispose();

                        // ORDEN PAGO
                        var sp = context.pInsertarOrdenPago(n_ord, po.status, DateTime.Now, po.cod_ben, po.descrip, po.forma_pag, DateTime.Now, null, null, user.ToUpper(),
                            null, null, null, po.tasa, po.co_mone, false, false, 0, null, null, null, null, null, null, null, null, null, null, user, sucur, "SERVER PROFIT WEB",
                            null, null);

                        // RENGLON
                        var sp_r = context.pInsertarRenglonesOrdenPago(1, n_ord, reng.co_cta_ingr_egr, null, reng.monto_d, 0, 0, 0, 0, 0, reng.tipo_imp, null, null, sucur,
                            user, null, null, "SERVER PROFIT WEB");

                        sp.Dispose();
                        sp_r.Dispose();

                        BoxMoves move = Box.AddExpense(n_ord, reng.monto_d, user, po.descrip);

                        tran.Commit();
                        new_order = GetPayOrderByID(n_ord);
                        new_order.campo1 = move.BoxID.ToString();
                        new_order.campo2 = move.ID.ToString();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        new_order.descrip = "ERROR";
                        new_order.campo1 = ex.Message;
                        Incident.CreateIncident("ERROR INTERNO AGREGANDO ORDEN DE PAGO", ex);
                    }
                }
            }

            return new_order;
        }
    }
}