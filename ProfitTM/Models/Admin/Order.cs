using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Order : ProfitAdmManager
    {
        public saPedidoVenta GetOrderByID(string id)
        {
            saPedidoVenta order = new saPedidoVenta();

            try
            {
                order = db.saPedidoVenta.Single(i => i.doc_num == id);
                order.saPedidoVentaReng = db.saPedidoVentaReng.Where(o => o.doc_num.Trim() == order.doc_num.Trim()).ToList();

                foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
                {
                    reng.saPedidoVenta = null;
                }
            }
            catch (Exception ex)
            {
                order = null;
            }

            return order;
        }

        public List<saPedidoVenta> GetAllOrders(int number, bool free)
        {
            List<saPedidoVenta> orders = new List<saPedidoVenta>();

            try
            {
                if (free)
                    orders = db.saPedidoVenta.Where(i => i.status.Trim() == "0").OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
                else
                    orders = db.saPedidoVenta.OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
    
                foreach (saPedidoVenta order in orders)
                {
                    order.saPedidoVentaReng = new OrderItem().GetRengsByOrder(order.doc_num.Trim());
                }
            }
            catch (Exception ex)
            {
                orders = null;
            }

            return orders;
        }

        public saPedidoVenta Delete(string id)
        {
            saPedidoVenta order = GetOrderByID(id);
            db.pEliminarPedidoVenta(order.doc_num, order.validador, "SERVER PROFIT WEB", "PROFIT WEB", null, order.rowguid);

            return order;
        }
    }
}