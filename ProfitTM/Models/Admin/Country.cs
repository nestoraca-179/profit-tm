using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Country
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static Country GetCountry(string connect, string ID)
        {
            Country country;

            string query = string.Format("SELECT * FROM saPais WHERE co_pais = '{0}'", ID);
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
                                country = new Country()
                                {
                                    ID = reader["co_pais"].ToString().Trim(),
                                    Name = reader["pais_des"].ToString()
                                };
                            }
                            else
                            {
                                country = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                country = null;
            }

            return country;
        }
    }
}