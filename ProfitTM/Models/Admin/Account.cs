using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Account : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }
        //public string Bank { get; set; }
        //public string Number { get; set; }
        //public string Currency { get; set; }

        //public static Account GetAccount(string connect, string ID)
        //{
        //    Account account;

        //    string query = string.Format("SELECT * FROM saCuentaIngEgr WHERE co_cta_ingr_egr = '{0}'", ID);
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand(query, conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        account = new Account()
        //                        {
        //                            ID = reader["co_cta_ingr_egr"].ToString().Trim(),
        //                            Name = reader["descrip"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        account = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        account = null;
        //    }

        //    return account;
        //}

        //public static Account GetBankAccount(string connect, string ID)
        //{
        //    Account account;

        //    string query = string.Format("SELECT * FROM saCuentaBancaria WHERE cod_cta = '{0}'", ID);
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand(query, conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        account = new Account()
        //                        {
        //                            ID = reader["cod_cta"].ToString().Trim(),
        //                            Bank = reader["co_ban"].ToString(),
        //                            Number = reader["num_cta"].ToString(),
        //                            Currency = reader["co_mone"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        account = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        account = null;
        //    }

        //    return account;
        //}

        //public static List<Account> GetAllAccounts(string connect)
        //{
        //    List<Account> accounts = new List<Account>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saCuentaIngEgr", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        accounts.Add(new Account()
        //                        {
        //                            ID = reader["co_cta_ingr_egr"].ToString(),
        //                            Name = reader["descrip"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        accounts = null;
        //    }

        //    return accounts;
        //}

        //public static List<Account> GetAllBankAccounts(string connect)
        //{
        //    List<Account> accounts = new List<Account>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saCuentaBancaria", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        accounts.Add(new Account()
        //                        {
        //                            ID = reader["cod_cta"].ToString(),
        //                            Bank = reader["co_ban"].ToString(),
        //                            Number = reader["num_cta"].ToString(),
        //                            Currency = reader["co_mone"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        accounts = null;
        //    }

        //    return accounts;
        //}
        #endregion

        public saCuentaIngEgr GetAccountByID(string id)
        {
            saCuentaIngEgr account = new saCuentaIngEgr();

            try
            {
                account = db.saCuentaIngEgr.SingleOrDefault(c => c.co_cta_ingr_egr == id);
            }
            catch (Exception ex)
            {
                account = null;
            }

            return account;
        }

        public saCuentaBancaria GetBankAccountByID(string id)
        {
            saCuentaBancaria bankAccount = new saCuentaBancaria();

            try
            {
                bankAccount = db.saCuentaBancaria.SingleOrDefault(c => c.cod_cta == id);
            }
            catch (Exception ex)
            {
                bankAccount = null;
            }

            return bankAccount;
        }

        public List<saCuentaIngEgr> GetAllAccounts()
        {
            List<saCuentaIngEgr> accounts = new List<saCuentaIngEgr>();

            try
            {
                accounts = db.saCuentaIngEgr.ToList();
            }
            catch (Exception ex)
            {
                accounts = null;
            }

            return accounts;
        }

        public List<saCuentaBancaria> GetAllBankAccounts()
        {
            List<saCuentaBancaria> accounts = new List<saCuentaBancaria>();

            try
            {
                accounts = db.saCuentaBancaria.ToList();
            }
            catch (Exception ex)
            {
                accounts = null;
            }

            return accounts;
        }
    }
}