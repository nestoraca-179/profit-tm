using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Zone : ProfitAdmManager
    {
        public saZona GetZoneByID(string id)
        {
            saZona zone;

            try
            {
                zone = db.saZona.AsNoTracking().SingleOrDefault(z => z.co_zon == id);
            }
            catch (Exception ex)
            {
                zone = null;
                Incident.CreateIncident("ERROR BUSCANDO ZONA " + id, ex);
            }

            return zone;
        }

        public List<saZona> GetAllZones()
        {
            List<saZona> zones;

            try
            {
                zones = db.saZona.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                zones = null;
                Incident.CreateIncident("ERROR BUSCANDO ZONAS", ex);
            }

            return zones;
        }
    }
}