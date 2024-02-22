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
            LogsFactOnline log = new LogsFactOnline()
            {
                NroFact = i.doc_num.Trim(),
                Status = 0,
                DateInserted = DateTime.Now
            };

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                db.LogsFactOnline.Add(log);
                db.SaveChanges();
            }

            return log;
        }

        public static List<LogsFactOnline> GetPendingLogs()
        {
            List<LogsFactOnline> logs;

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                logs = db.LogsFactOnline.AsNoTracking().Where(l => l.Status == 0).ToList();
            }

            return logs;
        }
    }
}