using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfitTM.Models
{
    public class Client
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Zone { get; set; }
        public string Account { get; set; }
        public string Country { get; set; }
        public string Segment { get; set; }
        public string RIF { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Amount { get; set; }
        public string Seller { get; set; }
        public string Cond { get; set; }
    }
}
