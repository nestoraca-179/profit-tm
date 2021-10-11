using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Bank
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static List<Bank> GetAllBanks(string connect)
        {
            List<Bank> banks = new List<Bank>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saBanco", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                banks.Add(new Bank()
                                {
                                    ID = reader["co_ban"].ToString(),
                                    Name = reader["des_ban"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                banks = null;
            }

            return banks;
        }
    }
}