using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class OrderItem : ProfitAdmManager
    {
        public List<saPedidoVentaReng> GetRengsByOrder(string doc)
        {
            List<saPedidoVentaReng> rengs = new List<saPedidoVentaReng>();

            try
            {
                rengs = db.saPedidoVentaReng.Where(r => r.doc_num == doc).ToList();
            }
            catch (Exception ex)
            {
                rengs = null;
            }

            return rengs;
        }
    }
}