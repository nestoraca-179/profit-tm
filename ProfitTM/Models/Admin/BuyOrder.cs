using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class BuyOrder : ProfitAdmManager
    {
        public static saOrdenCompra GetPayOrderByID(string id)
        {
            saOrdenCompra buyOrder;

            try
            {
                buyOrder = db.saOrdenCompra.AsNoTracking().Single(o => o.doc_num == id);
            }
            catch (Exception ex)
            {
                buyOrder = null;
                Incident.CreateIncident("ERROR BUSCANDO ORDEN DE COMPRA " + id, ex);
            }

            return buyOrder;
        }

        public List<saOrdenCompra> GetAllBuyOrders(int number)
        {
            List<saOrdenCompra> orders;

            try
            {
                orders = db.saOrdenCompra.AsNoTracking().Include("saOrdenCompraReng").Include("saProveedor").Include("saCondicionPago")
                    .OrderByDescending(i => i.fe_us_in).ThenBy(i => i.doc_num).Take(number).ToList();

                foreach (saOrdenCompra order in orders)
                {
                    order.saProveedor.saOrdenCompra = null;
                    order.saCondicionPago.saOrdenCompra = null;
                    foreach (saOrdenCompraReng reng in order.saOrdenCompraReng)
                    {
                        reng.saOrdenCompra = null;
                    }
                }
            }
            catch (Exception ex)
            {
                orders = null;
                Incident.CreateIncident("ERROR BUSCANDO ORDENES DE COMPRA", ex);
            }

            return orders;
        }
    }
}