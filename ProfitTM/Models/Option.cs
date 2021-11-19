using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Option
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ModuleID { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public bool Enabled { get; set; }

        public static List<Option> GetOptions(string moduleID, string userID)
        {
            List<Option> options = new List<Option>();
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(string.Format(@"select O.* from UserOptions UO
                        inner join Options O on UO.OptionID = O.ID
                        inner join Users U on UO.UserID = U.ID
                        where O.ModuleID = {0} and U.ID = {1}
                        order by O.Number", moduleID, userID), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Option option = new Option();

                                option.ID = reader["ID"].ToString();
                                option.Name = reader["OptionName"].ToString();
                                option.Icon = reader["Icon"].ToString();
                                option.URL = reader["URL"].ToString();
                                option.Enabled = bool.Parse(reader["Enabled"].ToString());

                                options.Add(option);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                options = null;
            }

            return options;
        }

        public static List<Option> GetAllOptions(string moduleID)
        {
            List<Option> options = new List<Option>();
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(string.Format("select * from Options where ModuleID = {0}", moduleID), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Option option = new Option();

                                option.ID = reader["ID"].ToString();
                                option.Name = reader["OptionName"].ToString();

                                options.Add(option);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                options = null;
            }

            return options;
        }
    }
}