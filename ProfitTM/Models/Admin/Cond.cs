using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Cond
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static Cond GetCond(string connect, string ID)
        {
            Cond cond;

            string query = string.Format("SELECT * FROM saCondicionPago WHERE co_cond = '{0}'", ID);
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
                                cond = new Cond()
                                {
                                    ID = reader["co_cond"].ToString().Trim(),
                                    Name = reader["cond_des"].ToString()
                                };
                            }
                            else
                            {
                                cond = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cond = null;
            }

            return cond;
        }
    }
}