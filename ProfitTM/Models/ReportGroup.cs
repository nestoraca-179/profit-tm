using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class ReportGroup
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<Report> Reports { get; set; }
    }
}