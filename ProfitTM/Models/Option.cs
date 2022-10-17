using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Option
    {
        public static List<Options> GetOptionsByUser(string moduleID, string userID)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Options> options = new List<Options>();

            try
            {
                List<Options> opts = db.Options.AsNoTracking().Where(o => o.ModuleID.ToString() == moduleID).ToList();
                List<UserOptions> userOptions = db.UserOptions.AsNoTracking().Where(uo => uo.UserID.ToString() == userID).OrderBy(uo => uo.OptionID).ToList();

                options = (from u in userOptions
                           join o in opts on u.OptionID equals o.ID
                           select new Options
                           {
                               ID = u.OptionID,
                               OptionName = o.OptionName,
                               OptionType = o.OptionType,
                               Icon = o.Icon,
                               URL = o.URL,
                               Enabled = o.Enabled,

                           }).ToList();
            }
            catch (Exception ex)
            {
                options = null;
                Incident.CreateIncident("ERROR BUSCANDO OPCIONES POR USUARIO", ex);
            }

            return options;
        }

        public static List<Options> GetOptionsByModule(string moduleID)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Options> options = new List<Options>();

            try
            {
                options = db.Options.AsNoTracking().Where(o => o.ModuleID.ToString() == moduleID).ToList();
            }
            catch (Exception ex)
            {
                options = null;
                Incident.CreateIncident("ERROR BUSCANDO OPCIONES POR MODULO", ex);
            }

            return options;
        }
    }
}