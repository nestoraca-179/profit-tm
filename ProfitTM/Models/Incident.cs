using System;

namespace ProfitTM.Models
{
    public class Incident
    {
        public static void CreateIncident(string titulo, Exception ex)
        {
            Incidents error = new Incidents();
            error.Titulo = titulo;
            error.Fecha = DateTime.Now;

            if (ex.InnerException != null)
            {
                Exception e = ex.InnerException;
                error.Descripcion = string.Format("{0} -> {1} -> {2}", e.Message, e.StackTrace, e.Source);
            }
            else
            {
                error.Descripcion = string.Format("{0} -> {1} -> {2}", ex.Message, ex.StackTrace, ex.Source);
            }

            using (ProfitTMEntities context = new ProfitTMEntities())
            {
                context.Incidents.Add(error);
                context.SaveChanges();
            }
        }
    }
}