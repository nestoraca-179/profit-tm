using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Order : ProfitAdmManager
    {
        public saPedidoVenta GetOrderByID(string id)
        {
            saPedidoVenta order;

            try
            {
                order = db.saPedidoVenta.AsNoTracking().Include("saPedidoVentaReng").Include("saCliente")
                    .Include("saCondicionPago").Include("saVendedor").Single(i => i.doc_num == id);

                order.saCliente.saPedidoVenta = null;
                order.saVendedor.saPedidoVenta = null;
                order.saCondicionPago.saPedidoVenta = null;
                foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
                {
                    reng.saPedidoVenta = null;
                }
            }
            catch (Exception ex)
            {
                order = null;
                Incident.CreateIncident("ERROR BUSCANDO PEDIDO " + id, ex);
            }

            return order;
        }

        public List<saPedidoVenta> GetAllOrders(int number, bool free)
        {
            List<saPedidoVenta> orders = new List<saPedidoVenta>();

            try
            {
                if (free)
                {
                    orders = db.saPedidoVenta.AsNoTracking().Include("saCliente").Where(o => o.status.Trim() == "0")
                        .OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
                }
                else
                {
                    orders = db.saPedidoVenta.AsNoTracking().Include("saPedidoVentaReng").Include("saCliente").Include("saVendedor")
                        .Include("saCondicionPago").OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
                }

                foreach (saPedidoVenta order in orders)
                {
                    if (!free)
                    {
                        order.saVendedor.saPedidoVenta = null;
                        order.saCondicionPago.saPedidoVenta = null;
                    }

                    order.saCliente.saPedidoVenta = null;
                    foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
                    {
                        reng.saPedidoVenta = null;
                    }
                }
            }
            catch (Exception ex)
            {
                orders = null;
                Incident.CreateIncident("ERROR BUSCANDO PEDIDOS", ex);
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