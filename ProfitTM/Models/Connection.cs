using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProfitTM.Models
{
    public class Connection
    {
        public string ID { get; set; }
        public string Server { get; set; }
        public string DB { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }

        public static List<Connection> GetConnections()
        {
            List<Connection> connections = new List<Connection>();
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from Connections", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Connection connection = new Connection();

                                connection.ID = reader["ID"].ToString();
                                connection.Server = reader["Server"].ToString();
                                connection.DB = reader["DB"].ToString();
                                connection.Username = reader["Username"].ToString();
                                connection.Password = reader["Password"].ToString();
                                connection.Type = reader["Type"].ToString();

                                connections.Add(connection);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                connections = null;
            }

            return connections;
        }
    }
}