using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Module
    {
        public static List<Modules> GetModulesByUser(string prod, string userID)
        {
            List<Modules> modules;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                List<Modules> mods = db.Modules.AsNoTracking().Where(m => m.Product == prod).ToList();
                List<UserModules> userModules = db.UserModules.AsNoTracking().Where(um => um.UserID.ToString() == userID).OrderBy(um => um.ModuleID).ToList();

                modules = (from u in userModules
                           join m in mods on u.ModuleID equals m.ID
                           select new Modules
                           {
                               ID = u.ModuleID,
                               ModuleName = m.ModuleName,
                               Icon = m.Icon,
                               ReportURL = m.ReportURL,
                               Options = Option.GetOptionsByUser(u.ModuleID.ToString(), userID),

                           }).ToList();
            }
            catch (Exception ex)
            {
                modules = null;
                Incident.CreateIncident("ERROR BUSCANDO MODULOS POR USUARIO", ex);
            }

            return modules;
        }

        public static List<Modules> GetAllModules()
        {
            List<Modules> modules;

            try
            {
                ProfitTMEntities db = new ProfitTMEntities();
                modules = db.Modules.AsNoTracking().ToList();
                foreach (Modules mod in modules)
                {
                    mod.Options = Option.GetOptionsByModule(mod.ID.ToString());
                }
            }
            catch (Exception ex)
            {
                modules = null;
                Incident.CreateIncident("ERROR BUSCANDO MODULOS", ex);
            }

            return modules;
        }
    }
}