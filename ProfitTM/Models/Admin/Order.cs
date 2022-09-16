using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class Order : ProfitAdmManager
    {
        public saPedidoVenta GetOrder(string id)
        {
            saPedidoVenta order = new saPedidoVenta();

            try
            {
                using (db)
                {
                    order = db.saPedidoVenta.SingleOrDefault(i => i.doc_num == id);
                    order.saPedidoVentaReng = db.saPedidoVentaReng.Where(o => o.doc_num.Trim() == order.doc_num.Trim()).ToList();
                    
                    foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
                    {
                        reng.saPedidoVenta = null;
                    }
                }
            }
            catch (Exception ex)
            {
                order = null;
            }

            return order;
        }
    }
}