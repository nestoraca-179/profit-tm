using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class Incident
    {
        public static List<Incidents> GetTopIncidents(int limit)
		{
            List<Incidents> incidents;

            using (ProfitTMEntities context = new ProfitTMEntities())
			{
                incidents = (from i in context.Incidents
                             where !i.Titulo.StartsWith("APPLICATION")
                             orderby i.Fecha descending
                             select i).Take(limit).ToList();
            }

            return incidents;
		}

        public static void CreateIncident(string titulo, Exception ex)
        {
            string user = "SYSTEM";
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["USER"] != null)
                        user = (HttpContext.Current.Session["USER"] as Users).Username;
                }
            }

            Incidents error = new Incidents() {
                Titulo = titulo,
                Fecha = DateTime.Now,
                Usuario = user
            };

            while (ex.InnerException != null)
                ex = ex.InnerException;

            error.Descripcion = string.Format("{0} -> {1} -> {2}", ex.Message, ex.StackTrace, ex.Source);
            using (ProfitTMEntities context = new ProfitTMEntities())
            {
                context.Incidents.Add(error);
                context.SaveChanges();
            }
        }
    }
}