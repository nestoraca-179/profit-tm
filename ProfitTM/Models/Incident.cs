using System;
using System.Web;

namespace ProfitTM.Models
{
    public class Incident
    {
        public static void CreateIncident(string titulo, Exception ex)
        {
            string user = "SYSTEM";
            
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["USER"] != null)
                    {
                        user = (HttpContext.Current.Session["USER"] as Users).Username;
                    }
                }
            }

            Incidents error = new Incidents();
            error.Titulo = titulo;
            error.Fecha = DateTime.Now;
            error.Usuario = user;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            error.Descripcion = string.Format("{0} -> {1} -> {2}", ex.Message, ex.StackTrace, ex.Source);

            using (ProfitTMEntities context = new ProfitTMEntities())
            {
                context.Incidents.Add(error);
                context.SaveChanges();
            }
        }
    }
}