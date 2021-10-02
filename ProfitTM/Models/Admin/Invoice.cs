using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class Invoice
    {
        public string ID { get; set; }
        public string Descrip { get; set; }
        public string Cond { get; set; }
        public string Seller { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
        public bool Printed { get; set; }
        public char Type { get; set; }
    }
}