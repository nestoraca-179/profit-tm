using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Stock { get; set; }
        public double Price { get; set; }

        public static List<Product> GetAllProducts(string connect)
        {
            List<Product> products = new List<Product>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saArticulo", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product()
                                {
                                    ID = reader["co_art"].ToString(),
                                    Name = reader["art_des"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                products = null;
            }

            return products;
        }
    }
}
