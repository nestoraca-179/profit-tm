using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class LogsFact
    {
        public static LogsFactOnline Add(saFacturaVenta i)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            LogsFactOnline log = new LogsFactOnline() 
            { 
                NroFact = i.doc_num,
                Status = 0,
                DateInserted = DateTime.Now
            };

            db.LogsFactOnline.Add(log);
            return log;
        }
    }
}