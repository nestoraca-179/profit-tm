using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Country : ProfitAdmManager
    {
        public saPais GetCountryByID(string id)
        {
            saPais country;

            try
            {
                country = db.saPais.AsNoTracking().SingleOrDefault(c => c.co_pais == id);
            }
            catch (Exception ex)
            {
                country = null;
                Incident.CreateIncident("ERROR BUSCANDO PAIS " + id, ex);
            }

            return country;
        }

        public List<saPais> GetAllCountries()
        {
            List<saPais> countries;

            try
            {
                countries = db.saPais.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                countries = null;
                Incident.CreateIncident("ERROR BUSCANDO PAISES", ex);
            }

            return countries;
        }
    }
}