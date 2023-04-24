using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Cond : ProfitAdmManager
    {
        public saCondicionPago GetCondByID(string id)
        {
            saCondicionPago cond;

            try
            {
                cond = db.saCondicionPago.AsNoTracking().SingleOrDefault(c => c.co_cond == id);
            }
            catch (Exception ex)
            {
                cond = null;
                Incident.CreateIncident("ERROR BUSCANDO CONDICION " + id, ex);
            }

            return cond;
        }

        public List<saCondicionPago> GetAllConds()
        {
            List<saCondicionPago> conds;

            try
            {
                conds = db.saCondicionPago.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                conds = null;
                Incident.CreateIncident("ERROR BUSCANDO CONDICIONES", ex);
            }

            return conds;
        }
    }
}