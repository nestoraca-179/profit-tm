using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProfitTM.Models
{
    public class Connection
    {
        public static Connections GetConnByID(string id)
        {
            Connections conn;

            try
            {
                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    conn = db.Connections.AsNoTracking().SingleOrDefault(c => c.ID.ToString() == id);
                }
            }
            catch (Exception ex)
            {
                conn = null;
                Incident.CreateIncident("ERROR BUSCANDO CONEXION " + id, ex);
            }

            return conn;
        }

        public static List<Connections> GetConnectionsByType(string type = "")
        {
            List<Connections> connections = new List<Connections>();

            try
            {
                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    if (!string.IsNullOrEmpty(type))
                        connections = db.Connections.AsNoTracking().Where(c => c.Type == type).ToList();
                    else
                        connections = db.Connections.AsNoTracking().ToList();
                }
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
            Connections newConn;

            try
            {
                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    newConn = db.Connections.Add(conn);
                    db.SaveChanges();
                }

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

        public static ProfitTMResponse Edit(Connections conn)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                using (ProfitTMEntities db = new ProfitTMEntities())
                {
                    db.Entry(conn).State = EntityState.Modified;
                    db.SaveChanges();
                }

                response.Status = "OK";
                response.Result = conn.ID;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR EDITANDO CONEXION", ex);
            }

            return response;
        }
    }
}