using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProfitTM.Models
{
	public class HttpTrace
	{
        public static HttpTraces GetTraceByID(int id)
        {
            HttpTraces trace;

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                trace = db.HttpTraces.AsNoTracking().Single(t => t.ID == id);
            }

            return trace;
        }

        public static List<HttpTraces> GetAllTraces()
        {
            List<HttpTraces> traces;

            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                traces = db.HttpTraces.AsNoTracking().OrderByDescending(t => t.Timestamp).Take(5000).ToList();
            }

            return traces;
        }

        public static async Task<HttpTraces> ParseToHttpTraceAsync(HttpRequestMessage req, HttpResponseMessage res = null, 
        TimeSpan? duration = null, Exception ex = null, string reqContent = null)
		{
            HttpTraces trace = new HttpTraces
            {
                Timestamp = DateTime.UtcNow,
                DurationMs = duration.HasValue ? (long)duration.Value.TotalMilliseconds : (long?)null,
                ExceptionMessage = ex?.Message
            };

            // Parsear Request
            if (req != null)
            {
                trace.RequestMethod = req.Method?.Method;
                trace.RequestUrl = req.RequestUri?.ToString();
                trace.RequestHeaders = SerializeHeaders(req.Headers);
                if (!string.IsNullOrEmpty(reqContent))
                    trace.RequestContent = reqContent.Length > 8000 ? reqContent.Substring(0, 8000) + "..." : reqContent;
                else if (req.Content != null)
                    trace.RequestContent = await ReadContentSafeAsync(req.Content);
            }

            // Parsear Response
            if (res != null)
            {
                trace.ResponseStatusCode = (int)res.StatusCode;
                trace.ResponseReasonPhrase = res.ReasonPhrase;
                trace.ResponseHeaders = SerializeHeaders(res.Headers);
                trace.IsSuccessStatusCode = res.IsSuccessStatusCode;
                if (res.Content != null)
                    trace.ResponseContent = await ReadContentSafeAsync(res.Content);
            }

            return trace;
        }

        private static string SerializeHeaders(HttpHeaders headers)
        {
            if (headers == null) 
                return null;
            
            try
            {
                Dictionary<string, string> headerDictionary = new Dictionary<string, string>();
                foreach (var header in headers)
                    headerDictionary[header.Key] = string.Join(", ", header.Value);
                
                return System.Text.Json.JsonSerializer.Serialize(headerDictionary);
            }
            catch
            {
                return "{}";
            }
        }

        private static async Task<string> ReadContentSafeAsync(HttpContent content)
        {
            try
            {
                // Guardar una copia del stream original para no afectar el contenido original
                var contentString = await content.ReadAsStringAsync();

                // Limitar el tamaño para evitar problemas con contenido muy grande
                const int maxContentLength = 8000;
                return contentString.Length > maxContentLength ? contentString.Substring(0, maxContentLength) + "..." : contentString;
            }
            catch (Exception ex)
            {
                return $"[Error reading content: {ex.Message}]";
            }
        }

        public static void AddTrace(HttpTraces trace)
		{
            using (ProfitTMEntities db = new ProfitTMEntities())
            {
                db.HttpTraces.Add(trace);
                db.SaveChanges();
            }
        }
    }
}