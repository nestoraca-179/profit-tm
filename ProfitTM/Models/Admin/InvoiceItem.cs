using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class InvoiceItem : ProfitAdmManager
    {
        public List<saFacturaVentaReng> GetRengsBySaleInvoice(string doc)
        {
            List<saFacturaVentaReng> rengs = new List<saFacturaVentaReng>();

            try
            {
                rengs = db.saFacturaVentaReng.Where(r => r.doc_num == doc).ToList();
            }
            catch (Exception ex)
            {
                rengs = null;
            }

            return rengs;
        }

        public List<saFacturaCompraReng> GetRengsByBuyInvoice(string doc)
        {
            List<saFacturaCompraReng> rengs = new List<saFacturaCompraReng>();

            try
            {
                rengs = db.saFacturaCompraReng.Where(r => r.doc_num == doc).ToList();
            }
            catch (Exception ex)
            {
                rengs = null;
            }

            return rengs;
        }
    }
}