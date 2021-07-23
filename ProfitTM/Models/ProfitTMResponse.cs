using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class ProfitTMResponse
    {
        public string Status { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
    }
}