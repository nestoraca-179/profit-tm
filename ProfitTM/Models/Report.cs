using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class Report
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Proc { get; set; }
        public string Cols { get; set; }
        public string Fields { get; set; }
        public string Format { get; set; }
        public string Params { get; set; }
        public string QueryParams { get; set; }
        public bool Enabled { get; set; }
        public string IDGroup { get; set; }
    }
}