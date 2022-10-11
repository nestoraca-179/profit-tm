using System;

namespace ProfitTM.Models
{
    public class Incident
    {
        public static void CreateIncident(string titulo, Exception ex)
        {
            Incidents error = new Incidents();

            error.Titulo = titulo;
            error.Descripcion = string.Format("{0} -> {1} -> {2}", ex.Message, ex.StackTrace, ex.Source);
            error.Fecha = DateTime.Now;

            using (ProfitTMEntities context = new ProfitTMEntities())
            {
                context.Incidents.Add(error);
                context.SaveChanges();
            }
        }
    }
}