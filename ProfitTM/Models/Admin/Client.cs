using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Client : Person
    {
        public static Client GetClient(string connect, string ID)
        {
            Client client;

            string query = string.Format("SELECT * FROM saCliente WHERE co_cli = '{0}'", ID);
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
                                client = new Client()
                                {
                                    ID = reader["co_cli"].ToString().Trim(),
                                    Name = reader["cli_des"].ToString(),
                                    Type = Type.GetTypeAdmin(connect, reader["tip_cli"].ToString(), "C"),
                                    Zone = Zone.GetZone(connect, reader["co_zon"].ToString()),
                                    Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
                                    Country = Country.GetCountry(connect, reader["co_pais"].ToString()),
                                    Segment = Segment.GetSegment(connect, reader["co_seg"].ToString()),
                                    RIF = reader["rif"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Phone = reader["telefonos"].ToString(),
                                    Address = reader["direc1"].ToString(),
                                    Seller = Seller.GetSeller(connect, reader["co_ven"].ToString()),
                                    Cond = Cond.GetCond(connect, reader["cond_pag"].ToString())
                                };
                            }
                            else
                            {
                                client = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                client = null;
            }

            return client;
        }

        public static List<Client> GetAllClients(string connect)
        {
            List<Client> clients = new List<Client>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saCliente", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clients.Add(new Client()
                                {
                                    ID = reader["co_cli"].ToString(),
                                    Name = reader["cli_des"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clients = null;
            }

            return clients;
        }
    }
}