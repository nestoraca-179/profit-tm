using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfitTM.Models
{
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Stock { get; set; }
        public double Price { get; set; }
    }
}
