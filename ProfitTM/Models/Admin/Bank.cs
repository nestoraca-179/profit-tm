using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Bank : ProfitAdmManager
    {
        public saBanco GetBankByID(string id)
        {
            saBanco bank;

            try
            {
                bank = db.saBanco.AsNoTracking().SingleOrDefault(c => c.co_ban == id);
            }
            catch (Exception ex)
            {
                bank = null;
                Incident.CreateIncident("ERROR BUSCANDO BANCO " + id, ex);
            }

            return bank;
        }

        public List<saBanco> GetAllbanks()
        {
            List<saBanco> banks;

            try
            {
                banks = db.saBanco.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                banks = null;
                Incident.CreateIncident("ERROR BUSCANDO BANCOS", ex);
            }

            return banks;
        }
    }
}