using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class InvoiceItem
    {
        public string Reng { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
        public double IVA { get; set; }
    }
}