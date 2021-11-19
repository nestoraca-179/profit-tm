using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class User
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Descrip { get; set; }
        public DateTime DateReg { get; set; }
        public bool IsAdm { get; set; }
        public bool IsCon { get; set; }
        public bool IsNom { get; set; }
        public bool Enabled { get; set; }

        public static User GetUser(string id)
        {
            User user;
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(string.Format("SELECT * FROM Users WHERE ID = {0}", id), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User()
                                {
                                    ID = reader["ID"].ToString(),
                                    Username = reader["Username"].ToString(),
                                    Descrip = reader["Descrip"].ToString(),
                                    DateReg = DateTime.Parse(reader["DateReg"].ToString()),
                                    IsAdm = bool.Parse(reader["IsAdm"].ToString()),
                                    IsCon = bool.Parse(reader["IsCon"].ToString()),
                                    IsNom = bool.Parse(reader["IsNom"].ToString()),
                                    Enabled = bool.Parse(reader["Enabled"].ToString())
                                };
                            }
                            else
                            {
                                user = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                user = null;
            }

            return user;
        }

        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("SELECT * FROM Users", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User();

                                user.ID = reader["ID"].ToString();
                                user.Username = reader["Username"].ToString();
                                user.Descrip = reader["Descrip"].ToString();
                                user.DateReg = DateTime.Parse(reader["DateReg"].ToString());
                                user.IsAdm = bool.Parse(reader["IsAdm"].ToString());
                                user.IsCon = bool.Parse(reader["IsCon"].ToString());
                                user.IsNom = bool.Parse(reader["IsNom"].ToString());
                                user.Enabled = bool.Parse(reader["Enabled"].ToString());

                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                users = null;
            }

            return users;
        }
    }
}