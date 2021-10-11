using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Zone
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static Zone GetZone(string connect, string ID)
        {
            Zone zone;

            string query = string.Format("SELECT * FROM saZona WHERE co_zon = '{0}'", ID);
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
                                zone = new Zone()
                                {
                                    ID = reader["co_zon"].ToString().Trim(),
                                    Name = reader["zon_des"].ToString()
                                };
                            }
                            else
                            {
                                zone = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                zone = null;
            }

            return zone;
        }

        public static List<Zone> GetAllZones(string connect)
        {
            List<Zone> zones = new List<Zone>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saZona", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                zones.Add(new Zone()
                                {
                                    ID = reader["co_zon"].ToString(),
                                    Name = reader["zon_des"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                zones = null;
            }

            return zones;
        }
    }
}