using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ProfitTM.Models
{
    public class LogsFact
    {
        private const int PROCESSING_TIMEOUT_MINUTES = 15;
        private const int SQL_COMMAND_TIMEOUT_SECONDS = 15;

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
                logs = db.LogsFactOnline.AsNoTracking().Where(l => l.Status != (int)LogStatus.SENTSTATUS && l.Status != (int)LogStatus.CANCELLEDSTATUS && l.Status != (int)LogStatus.PROCESSINGSTATUS)
                    .OrderBy(l => l.DateInserted).ThenBy(l => l.NroFact).ToList();
            }

            return logs;
        }

        public static List<LogsFactOnline> ClaimPendingLogs(int batchSize = 200)
        {
            List<LogsFactOnline> claimed = new List<LogsFactOnline>();

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                db.Database.CommandTimeout = SQL_COMMAND_TIMEOUT_SECONDS;
                RecoverStaleProcessingLogs(db);

                var candidates = db.LogsFactOnline.AsNoTracking()
                    .Where(l => l.Status != (int)LogStatus.SENTSTATUS && l.Status != (int)LogStatus.CANCELLEDSTATUS && l.Status != (int)LogStatus.PROCESSINGSTATUS)
                    .OrderBy(l => l.DateInserted)
                    .ThenBy(l => l.NroFact)
                    .Take(batchSize)
                    .ToList();

                foreach (LogsFactOnline candidate in candidates)
                {
                    DateTime triedAt = DateTime.Now;
                    int affected = 0;

                    try
                    {
                        affected = db.Database.ExecuteSqlCommand(@"
                            UPDATE LogsFactOnline
                            SET Status = @processingStatus,
                                Times = ISNULL(Times, 0) + 1,
                                DateTried = @dateTried,
                                Message = @message
                            WHERE NroFact = @nroFact
                              AND ConnID = @connId
                              AND Status = @currentStatus",
                            new SqlParameter("@processingStatus", (int)LogStatus.PROCESSINGSTATUS),
                            new SqlParameter("@dateTried", triedAt),
                            new SqlParameter("@message", "PROCESSING"),
                            new SqlParameter("@nroFact", candidate.NroFact),
                            new SqlParameter("@connId", candidate.ConnID),
                            new SqlParameter("@currentStatus", candidate.Status));
                    }
                    catch (Exception ex)
                    {
                        Incident.CreateIncident($"ERROR RECLAMANDO LOG {candidate.NroFact} ({candidate.ConnID})", ex);
                        continue;
                    }

                    if (affected == 1)
                    {
                        candidate.Times++;
                        candidate.DateTried = triedAt;
                        candidate.Message = "PROCESSING";
                        claimed.Add(candidate);
                    }
                }
            }

            return claimed;
        }

        private static void RecoverStaleProcessingLogs(ProfitTMEntities db)
        {
            DateTime cutoff = DateTime.Now.AddMinutes(-PROCESSING_TIMEOUT_MINUTES);

            try
            {
                bool hasStaleProcessingLogs = db.LogsFactOnline.AsNoTracking().Any(l =>
                    l.Status == (int)LogStatus.PROCESSINGSTATUS &&
                    l.DateTried != null &&
                    l.DateTried < cutoff);

                if (!hasStaleProcessingLogs)
                    return;

                int recovered = db.Database.ExecuteSqlCommand(@"
                    UPDATE LogsFactOnline
                    SET Status = @pendingStatus,
                        Message = @message
                    WHERE Status = @processingStatus
                      AND DateTried IS NOT NULL
                      AND DateTried < @cutoff",
                    new SqlParameter("@pendingStatus", (int)LogStatus.PENDINGSTATUS),
                    new SqlParameter("@message", "RECOVERED AFTER PROCESSING TIMEOUT"),
                    new SqlParameter("@processingStatus", (int)LogStatus.PROCESSINGSTATUS),
                    new SqlParameter("@cutoff", cutoff));

                if (recovered > 0)
                    CreateLogInFile($"[RECOVERY] Se recuperaron {recovered} logs atascados en PROCESANDO.");
            }
            catch (Exception ex)
            {
                Incident.CreateIncident("ERROR RECOVERING STALE PROCESSING LOGS", ex);
                CreateLogInFile("[RECOVERY] La recuperacion de logs atascados fallo; se continuara con el reclamo de pendientes.");
            }
        }

        public static LogsFactOnline Add(saFacturaVenta i, int conn, string json, string serie)
        {
            LogsFactOnline log = new LogsFactOnline()
            {
                NroFact = i.doc_num.Trim(),
                Serie = serie,
                ConnID = conn,
                BodyJson = json,
                Status = (int)LogStatus.PENDINGSTATUS,
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
                throw;
            }

            return log;
        }

        public static bool IsBlockedForManualEdit(int status)
        {
            return status == (int)LogStatus.SENTSTATUS || status == (int)LogStatus.CANCELLEDSTATUS || status == (int)LogStatus.PROCESSINGSTATUS || status == (int)LogStatus.CONTROLPENDINGSTATUS;
        }

        public static bool RequiresControlSync(LogsFactOnline log)
        {
            return log != null && log.Status == (int)LogStatus.CONTROLPENDINGSTATUS && !string.IsNullOrWhiteSpace(log.NroControl);
        }

        public static void CreateProcessingTrace(LogsFactOnline log, string stage, string message)
        {
            string doc = log == null ? "N/A" : log.NroFact;
            string conn = log == null ? "N/A" : log.ConnID.ToString();
            string attempts = log == null ? "N/A" : log.Times.ToString();
            CreateLogInFile($"[{stage}] DOC={doc} CONN={conn} ATTEMPT={attempts} {message}");
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

        public static void CreateInvoiceLog(string doc_num, saFacturaVenta invoice, bool error)
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}/Logs/";
            string file = $"{path}/InvoiceLogs.log";
            string invoiceBody = JsonConvert.SerializeObject(invoice);
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{(error ? "ERROR" : "INFO")}] {doc_num} -> {invoiceBody}";

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    writer.WriteLine(logEntry);
                    writer.WriteLine(new string('-', 80));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir en el log: {ex.Message}");
            }
        }
    }
}