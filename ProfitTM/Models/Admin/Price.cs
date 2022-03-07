using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Price : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static List<Price> GetAllPrices(string connect)
        //{
        //    List<Price> prices = new List<Price>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saTipoPrecio", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        prices.Add(new Price()
        //                        {
        //                            ID = reader["co_precio"].ToString(),
        //                            Name = reader["des_precio"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        prices = null;
        //    }

        //    return prices;
        //}
        #endregion

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