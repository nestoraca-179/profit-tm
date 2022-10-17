using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Connection
    {
        public static List<Connections> GetConnectionsByType(string type = "")
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Connections> connections = new List<Connections>();

            try
            {
                if (!string.IsNullOrEmpty(type))
                    connections = db.Connections.AsNoTracking().Where(c => c.Type == type).ToList();
                else
                    connections = db.Connections.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                connections = null;
                Incident.CreateIncident("ERROR BUSCANDO CONEXIONES", ex);
            }

            return connections;
        }

        public static ProfitTMResponse Add(Connections conn)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            ProfitTMEntities db = new ProfitTMEntities();
            Connections newConn;

            try
            {
                newConn = db.Connections.Add(conn);
                db.SaveChanges();

                response.Status = "OK";
                response.Result = newConn.ID;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO CONEXION", ex);
            }

            return response;
        }
    }
}