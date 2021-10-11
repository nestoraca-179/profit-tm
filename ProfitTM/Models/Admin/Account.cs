using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Account
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Bank { get; set; }
        public string Number { get; set; }
        public string Currency { get; set; }

        public static Account GetAccount(string connect, string ID)
        {
            Account account;

            string query = string.Format("SELECT * FROM saCuentaIngEgr WHERE co_cta_ingr_egr = '{0}'", ID);
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
                                account = new Account()
                                {
                                    ID = reader["co_cta_ingr_egr"].ToString().Trim(),
                                    Name = reader["descrip"].ToString()
                                };
                            }
                            else
                            {
                                account = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                account = null;
            }

            return account;
        }

        public static Account GetBankAccount(string connect, string ID)
        {
            Account account;

            string query = string.Format("SELECT * FROM saCuentaBancaria WHERE cod_cta = '{0}'", ID);
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
                                account = new Account()
                                {
                                    ID = reader["cod_cta"].ToString().Trim(),
                                    Bank = reader["co_ban"].ToString(),
                                    Number = reader["num_cta"].ToString(),
                                    Currency = reader["co_mone"].ToString()
                                };
                            }
                            else
                            {
                                account = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                account = null;
            }

            return account;
        }

        public static List<Account> GetAllAccounts(string connect)
        {
            List<Account> accounts = new List<Account>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saCuentaIngEgr", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Account()
                                {
                                    ID = reader["co_cta_ingr_egr"].ToString(),
                                    Name = reader["descrip"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                accounts = null;
            }

            return accounts;
        }

        public static List<Account> GetAllBankAccounts(string connect)
        {
            List<Account> accounts = new List<Account>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saCuentaBancaria", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Account()
                                {
                                    ID = reader["cod_cta"].ToString(),
                                    Bank = reader["co_ban"].ToString(),
                                    Number = reader["num_cta"].ToString(),
                                    Currency = reader["co_mone"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                accounts = null;
            }

            return accounts;
        }
    }
}