using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Currency
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Exchange { get; set; }

        public static Currency GetCurrency(string connect, string ID)
        {
            Currency currency;

            string query = string.Format("SELECT * FROM saMoneda WHERE co_mone = '{0}'", ID);
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
                                currency = new Currency()
                                {
                                    ID = reader["co_mone"].ToString().Trim(),
                                    Name = reader["mone_des"].ToString(),
                                    Exchange = double.Parse(reader["cambio"].ToString())
                                };
                            }
                            else
                            {
                                currency = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                currency = null;
            }

            return currency;
        }

        public static List<Currency> GetAllCurrencies(string connect)
        {
            List<Currency> currencies = new List<Currency>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saMoneda", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                currencies.Add(new Currency()
                                {
                                    ID = reader["co_mone"].ToString().Trim(),
                                    Name = reader["mone_des"].ToString(),
                                    Exchange = double.Parse(reader["cambio"].ToString())
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                currencies = null;
            }

            return currencies;
        }
    }
}