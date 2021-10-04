using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class Module
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Product { get; set; }
        public string UrlTable { get; set; }
        public string UrlProcess { get; set; }
        public string UrlReport { get; set; }
    }
}