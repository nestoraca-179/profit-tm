using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Price
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static List<Price> GetAllPrices(string connect)
        {
            List<Price> prices = new List<Price>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saTipoPrecio", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                prices.Add(new Price()
                                {
                                    ID = reader["co_precio"].ToString(),
                                    Name = reader["des_precio"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                prices = null;
            }

            return prices;
        }
    }
}