using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class ReportTree
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Prod { get; set; }
        public List<ReportGroup> ReportGroups { get; set; }
    }
}