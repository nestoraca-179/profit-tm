using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Module
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Product { get; set; }
        public string ReportURL { get; set; }
        public List<Option> Options { get; set; }

        public static List<Module> GetModules(string prod, string userID)
        {
            List<Module> modules = new List<Module>();
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(string.Format(@"select M.* from UserModules UM
                        inner join Modules M on UM.ModuleID = M.ID
                        inner join Users U on UM.UserID = U.ID
                        where M.Product = '{0}' and U.ID = {1}", prod, userID), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Module module = new Module();

                                module.ID = reader["ID"].ToString();
                                module.Name = reader["ModuleName"].ToString();
                                module.Icon = reader["Icon"].ToString();
                                module.ReportURL = reader["ReportURL"].ToString();
                                module.Options = Option.GetOptions(reader["ID"].ToString(), userID);

                                modules.Add(module);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                modules = null;
            }

            return modules;
        }
    
        public static List<Module> GetAllModules()
        {
            List<Module> modules = new List<Module>();
            string DBMain = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBMain))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from Modules", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Module module = new Module();

                                module.ID = reader["ID"].ToString();
                                module.Name = reader["ModuleName"].ToString();
                                module.Product = reader["Product"].ToString();
                                module.Options = Option.GetAllOptions(module.ID);

                                modules.Add(module);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                modules = null;
            }

            return modules;
        }
    }
}