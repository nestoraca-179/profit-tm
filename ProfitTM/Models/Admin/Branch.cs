using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Branch : ProfitAdmManager
    {
        public static bool Exists(string id, string n_entity)
        {
            using (ProfitAdmEntities context = new ProfitAdmEntities(n_entity))
                return context.saSucursal.AsNoTracking().Any(s => s.co_sucur.Trim() == id.Trim());
        }

        public saSucursal GetBranchByID(string id)
        {
            saSucursal branch;

            try
            {
                branch = db.saSucursal.AsNoTracking().SingleOrDefault(s => s.co_sucur.Trim() == id.Trim());
            }
            catch (Exception ex)
            {
                branch = null;
                Incident.CreateIncident("ERROR BUSCANDO SUCURSAL " + id, ex);
            }

            return branch;
        }

        public List<saSucursal> GetAllBranchs()
        {
            List<saSucursal> branchs;

            try
            {
                branchs = db.saSucursal.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                branchs = null;
                Incident.CreateIncident("ERROR BUSCANDO SUCURSALES", ex);
            }

            return branchs;
        }

        public bool UseBranchs()
        {
            return db.par_emp.AsNoTracking().First().v_maneja_sucursales;
        }
    }
}