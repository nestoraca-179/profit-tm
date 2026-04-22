using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProfitTM.Models
{
    public class Connection
    {
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> tokenLocks = new ConcurrentDictionary<int, SemaphoreSlim>();

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
            List<Connections> connections;

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

            try
            {
                Connections newConn;
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

        public static async Task<Connections> EnsureValidTokenAsync(Connections conn)
        {
            if (conn == null)
                throw new Exception("Conexion no encontrada");

            if (!conn.UseFactOnline)
                return conn;

            if (HasValidToken(conn))
                return conn;

            SemaphoreSlim tokenLock = tokenLocks.GetOrAdd(conn.ID, _ => new SemaphoreSlim(1, 1));
            await tokenLock.WaitAsync();

            try
            {
                Connections currentConn = GetConnByID(conn.ID.ToString());
                if (currentConn == null)
                    throw new Exception($"Conexion {conn.ID} no encontrada");

                if (HasValidToken(currentConn))
                    return currentConn;

                ModelAuthRequest auth = new ModelAuthRequest()
                {
                    usuario = currentConn.UserToken,
                    clave = currentConn.PassToken
                };

                ModelAuthResponse response = await new Root().SendAuth(auth);
                if (response.codigo != 200)
                    throw new AuthenticationException(response.mensaje);

                currentConn.Token = response.token;
                currentConn.DateToken = response.expiracion.AddHours(-4);

                ProfitTMResponse editResult = Edit(currentConn);
                if (editResult.Status != "OK")
                    throw new Exception(editResult.Message ?? $"No se pudo actualizar el token de la conexion {currentConn.ID}");

                return currentConn;
            }
            finally
            {
                tokenLock.Release();
            }
        }

        private static bool HasValidToken(Connections conn)
        {
            return conn != null && !string.IsNullOrEmpty(conn.Token) && conn.DateToken != null && DateTime.Now <= conn.DateToken;
        }
    }
}