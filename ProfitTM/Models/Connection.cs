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
                    connections = db.Connections.Where(c => c.Type == type).ToList();
                else
                    connections = db.Connections.ToList();
            }
            catch (Exception ex)
            {
                connections = null;
            }

            return connections;
        }

        public static ProfitTMResponse Add(Connections conn)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            ProfitTMResponse response = new ProfitTMResponse();
            Connections newConn = new Connections();

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
            }

            return response;
        }
    }
}