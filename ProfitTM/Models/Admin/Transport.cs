using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Transport
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static Transport GetTransport(string connect, string ID)
        {
            Transport transport;

            string query = string.Format("SELECT * FROM saTransporte WHERE co_tran = '{0}'", ID);
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
                                transport = new Transport()
                                {
                                    ID = reader["co_tran"].ToString().Trim(),
                                    Name = reader["des_tran"].ToString()
                                };
                            }
                            else
                            {
                                transport = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                transport = null;
            }

            return transport;
        }

        public static List<Transport> GetAllTransports(string connect)
        {
            List<Transport> transports = new List<Transport>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saTransporte", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transports.Add(new Transport()
                                {
                                    ID = reader["co_tran"].ToString(),
                                    Name = reader["des_tran"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                transports = null;
            }

            return transports;
        }
    }
}