using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Storage
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static Storage GetStorage(string connect, string ID)
        {
            Storage storage;

            string query = string.Format("SELECT * FROM saAlmacen WHERE co_alma = '{0}'", ID);
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
                                storage = new Storage()
                                {
                                    ID = reader["co_alma"].ToString().Trim(),
                                    Name = reader["des_alma"].ToString()
                                };
                            }
                            else
                            {
                                storage = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                storage = null;
            }

            return storage;
        }

        public static List<Storage> GetAllStorages(string connect)
        {
            List<Storage> storages = new List<Storage>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saAlmacen", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                storages.Add(new Storage()
                                {
                                    ID = reader["co_alma"].ToString(),
                                    Name = reader["des_alma"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                storages = null;
            }

            return storages;
        }
    }
}