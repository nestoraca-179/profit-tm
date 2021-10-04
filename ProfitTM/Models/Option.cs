using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class Option
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public char Type { get; set; }
        public string Function { get; set; }
    }
}