using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Module
    {
        public static List<Modules> GetModulesByUser(string prod, string userID)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Modules> modules = new List<Modules>();

            try
            {
                List<Modules> mods = db.Modules.Where(m => m.Product == prod).ToList();
                List<UserModules> userModules = db.UserModules.Where(um => um.UserID.ToString() == userID).OrderBy(um => um.ModuleID).ToList();

                modules = (from u in userModules
                           join m in mods on u.ModuleID equals m.ID
                           select new Modules
                           {
                               ID = u.ModuleID,
                               ModuleName = m.ModuleName,
                               Icon = m.Icon,
                               ReportURL = m.ReportURL,
                               Options = Option.GetOptionsByUser(u.ModuleID.ToString(), userID),

                           }).ToList();

                #region CODIGO ANTERIOR
                /*using (SqlConnection conn = new SqlConnection(DBMain))
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
                }*/
                #endregion
            }
            catch (Exception ex)
            {
                modules = null;
            }

            return modules;
        }

        public static List<Modules> GetAllModules()
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Modules> modules = new List<Modules>();

            try
            {
                modules = db.Modules.ToList();
                foreach (Modules mod in modules)
                {
                    mod.Options = Option.GetOptionsByModule(mod.ID.ToString());
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