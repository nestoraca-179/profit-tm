using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Storage : ProfitAdmManager
    {
        public saAlmacen GetAlmByID(string id)
        {
            saAlmacen alm;

            try
            {
                alm = db.saAlmacen.AsNoTracking().SingleOrDefault(c => c.co_alma == id);
            }
            catch (Exception ex)
            {
                alm = null;
                Incident.CreateIncident("ERROR BUSCANDO ALMACEN " + id, ex);
            }

            return alm;
        }

        public List<saAlmacen> GetAllAlms()
        {
            List<saAlmacen> alms;

            try
            {
                alms = db.saAlmacen.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                alms = null;
                Incident.CreateIncident("ERROR BUSCANDO ALMACENES", ex);
            }

            return alms;
        }
    }
}