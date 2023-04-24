using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Currency : ProfitAdmManager
    {
        public saMoneda GetcurrencyByID(string id)
        {
            saMoneda currency;

            try
            {
                currency = db.saMoneda.AsNoTracking().SingleOrDefault(c => c.co_mone == id);
            }
            catch (Exception ex)
            {
                currency = null;
                Incident.CreateIncident("ERROR BUSCANDO MONEDA " + id, ex);
            }

            return currency;
        }

        public List<saMoneda> GetAllCurrencies()
        {
            List<saMoneda> currencies;

            try
            {
                currencies = db.saMoneda.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                currencies = null;
                Incident.CreateIncident("ERROR BUSCANDO MONEDAS", ex);
            }

            return currencies;
        }
    }
}