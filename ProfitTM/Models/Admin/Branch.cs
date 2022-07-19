using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Branch : ProfitAdmManager
    {
        public saSucursal GetBranchByID(string id)
        {
            saSucursal branch = new saSucursal();

            try
            {
                branch = db.saSucursal.SingleOrDefault(s => s.co_sucur == id);
            }
            catch (Exception ex)
            {
                branch = null;
            }

            return branch;
        }

        public List<saSucursal> GetAllBranchs()
        {
            List<saSucursal> branchs = new List<saSucursal>();

            try
            {
                branchs = db.saSucursal.ToList();
            }
            catch (Exception ex)
            {
                branchs = null;
            }

            return branchs;
        }
    
        public bool UseBranchs()
        {
            return db.par_emp.First().v_maneja_sucursales;
        }
    }
}