using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Price : ProfitAdmManager
    {
        public saTipoPrecio GetPriceByID(string id)
        {
            saTipoPrecio price = new saTipoPrecio();

            try
            {
                price = db.saTipoPrecio.SingleOrDefault(c => c.co_precio == id);
            }
            catch (Exception ex)
            {
                price = null;
            }

            return price;
        }

        public List<saTipoPrecio> GetAllPrices()
        {
            List<saTipoPrecio> prices = new List<saTipoPrecio>();

            try
            {
                prices = db.saTipoPrecio.ToList();
            }
            catch (Exception ex)
            {
                prices = null;
            }

            return prices;
        }
    }
}