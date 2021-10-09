using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Seller
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public char Type { get; set; }
        public string Number { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public static Seller GetSeller(string connect, string ID)
        {
            Seller seller;

            string query = string.Format("SELECT * FROM saVendedor WHERE co_ven = '{0}'", ID);
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                seller = new Seller()
                                {
                                    ID = reader["co_ven"].ToString().Trim(),
                                    Name = reader["ven_des"].ToString(),
                                    Type = Convert.ToChar(reader["tipo"].ToString().Trim()),
                                    Number = reader["cedula"].ToString().Trim(),
                                    Phone = reader["telefonos"].ToString(),
                                    Address = reader["direc1"].ToString()
                                };
                            }
                            else
                            {
                                seller = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                seller = null;
            }

            return seller;
        }
    }
}