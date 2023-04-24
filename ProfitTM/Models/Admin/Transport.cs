using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Transport : ProfitAdmManager
    {
        public saTransporte GetTransportByID(string id)
        {
            saTransporte transport;

            try
            {
                transport = db.saTransporte.AsNoTracking().SingleOrDefault(c => c.co_tran == id);
            }
            catch (Exception ex)
            {
                transport = null;
                Incident.CreateIncident("ERROR BUSCANDO TRANSPORTE " + id, ex);
            }

            return transport;
        }

        public List<saTransporte> GetAllTransports()
        {
            List<saTransporte> transports;

            try
            {
                transports = db.saTransporte.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                transports = null;
                Incident.CreateIncident("ERROR BUSCANDO TRANSPORTES", ex);
            }

            return transports;
        }
    }
}