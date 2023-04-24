using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Seller : ProfitAdmManager
    {
        public saVendedor GetSellerByID(string id)
        {
            saVendedor seller;

            try
            {
                seller = db.saVendedor.AsNoTracking().SingleOrDefault(s => s.co_ven == id);
            }
            catch (Exception ex)
            {
                seller = null;
                Incident.CreateIncident("ERROR BUSCANDO VENDEDOR " + id, ex);
            }

            return seller;
        }

        public List<saVendedor> GetAllSellers()
        {
            List<saVendedor> sellers;

            try
            {
                sellers = db.saVendedor.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                sellers = null;
                Incident.CreateIncident("ERROR BUSCANDO VENDEDORES", ex);
            }

            return sellers;
        }
    }
}