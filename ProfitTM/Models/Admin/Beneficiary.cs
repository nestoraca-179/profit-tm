using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Beneficiary : ProfitAdmManager
    {
        public saBeneficiario GetBeneficiaryByID(string id)
        {
            saBeneficiario benef = new saBeneficiario();

            try
            {
                benef = db.saBeneficiario.AsNoTracking().SingleOrDefault(b => b.cod_ben == id);
            }
            catch (Exception ex)
            {
                benef = null;
                Incident.CreateIncident("ERROR BUSCANDO BENEFICIARIO " + id, ex);
            }

            return benef;
        }

        public List<saBeneficiario> GetAllBeneficiaries()
        {
            List<saBeneficiario> benefs;

            try
            {
                benefs = db.saBeneficiario.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                benefs = null;
                Incident.CreateIncident("ERROR BUSCANDO BENEFICIARIOS", ex);
            }

            return benefs;
        }
    }
}