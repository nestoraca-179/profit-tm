using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Price : ProfitAdmManager
    {
        public saTipoPrecio GetPriceByID(string id)
        {
            saTipoPrecio price;

            try
            {
                price = db.saTipoPrecio.AsNoTracking().SingleOrDefault(c => c.co_precio == id);
            }
            catch (Exception ex)
            {
                price = null;
                Incident.CreateIncident("ERROR BUSCANDO TIPO DE PRECIO " + id, ex);
            }

            return price;
        }

        public List<saTipoPrecio> GetAllPrices()
        {
            List<saTipoPrecio> prices;

            try
            {
                prices = db.saTipoPrecio.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                prices = null;
                Incident.CreateIncident("ERROR BUSCANDO TIPOS DE PRECIO", ex);
            }

            return prices;
        }
    }
}