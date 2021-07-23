using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class ReportGroup
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<Report> Reports { get; set; }
    }
}