using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ProfitTM.Models
{
    public class Connection
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string DB { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }

        public static List<Connection> GetConnections(string prod = "")
        {
            List<Connection> connections = new List<Connection>();
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString, query = "";

            switch (prod)
            {
                case "Admin":
                    prod = "ADM";
                    break;
                case "Cont":
                    prod = "CON";
                    break;
                case "Nomi":
                    prod = "NOM";
                    break;
            }

            if (!string.IsNullOrEmpty(prod))
            {
                query = string.Format("select * from Connections where Type = '{0}'", prod);
            }
            else
            {
                query = "select * from Connections";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Connection connection = new Connection();

                                connection.ID = reader["ID"].ToString();
                                connection.Name = reader["Name"].ToString();
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
    
        public static ProfitTMResponse SaveConn(string name, string server, string db, string user, string pass, string type)
        {
            Connection connection = new Connection();
            StringBuilder query = new StringBuilder();
            ProfitTMResponse response = new ProfitTMResponse();

            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            switch (type)
            {
                case "Admin":
                    type = "ADM";
                    break;
                case "Cont":
                    type = "CON";
                    break;
                case "Nomi":
                    type = "NOM";
                    break;
            }

            query.Append("insert into Connections (Name, Server, DB, Username, Password, Type) \n");
            query.AppendFormat("values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", name, server, db, user, pass, type);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        comm.ExecuteNonQuery();

                        connection.Name = name;
                        connection.Server = server;
                        connection.DB = db;
                        connection.Username = user;
                        connection.Password = pass;
                        connection.Type = type;
                    }
                }

                response.Status = "OK";
                response.Result = connection;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }
    }
}