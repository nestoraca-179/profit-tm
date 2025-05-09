﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace ProfitTM.Models
{
    public class LogsFact
    {
        public static LogsFactOnline GetLogByID(string l, int conn)
        {
            LogsFactOnline log;

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
				log = db.LogsFactOnline.AsNoTracking().Single(lo => lo.NroFact == l && lo.ConnID == conn);
			}

            return log;
        }

        public static List<LogsFactOnline> GetAllLogs(int conn)
        {
            List<LogsFactOnline> logs;

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                logs = db.LogsFactOnline.AsNoTracking().Where(l => l.ConnID == conn).OrderByDescending(l => l.DateInserted).Take(5000).ToList();
            }

            return logs;
        }

        public static List<LogsFactOnline> GetPendingLogs()
        {
            List<LogsFactOnline> logs;

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                logs = db.LogsFactOnline.AsNoTracking().Where(l => l.Status != 1 && l.Status != 4).OrderBy(l => l.DateInserted).ThenBy(l => l.NroFact).ToList();
            }

            return logs;
        }

        public static LogsFactOnline Add(saFacturaVenta i, int conn, string json, string serie)
        {
            LogsFactOnline log = new LogsFactOnline()
            {
                NroFact = i.doc_num.Trim(),
                Serie = serie,
                ConnID = conn,
                BodyJson = json,
                Status = 0,
                Times = 0,
                DateInserted = DateTime.Now
            };

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                db.LogsFactOnline.Add(log);
                db.SaveChanges();
            }

            return log;
        }

        public static LogsFactOnline Edit(LogsFactOnline log)
        {
            try
            {
                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    db.Entry(log).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Incident.CreateIncident("ERROR EDITANDO LOG", ex);
                throw ex;
            }

            return log;
        }
    
        public static void CreateLogInFile(string message)
		{
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}/Logs/";
            string file = $"{path}/Logs.txt";
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [INFO] {message}";

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (StreamWriter writer = new StreamWriter(file, true))
                    writer.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir en el log: {ex.Message}");
            }
        }
    }
}