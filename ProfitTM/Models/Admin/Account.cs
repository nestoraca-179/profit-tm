using System;
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
    }
}