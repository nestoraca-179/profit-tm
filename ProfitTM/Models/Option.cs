using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Option
    {
        public static List<Options> GetOptionsByUser(string moduleID, string userID)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Options> options = new List<Options>();

            try
            {
                List<Options> opts = db.Options.Where(o => o.ModuleID.ToString() == moduleID).ToList();
                List<UserOptions> userOptions = db.UserOptions.Where(uo => uo.UserID.ToString() == userID).OrderBy(uo => uo.OptionID).ToList();

                options = (from u in userOptions
                           join o in opts on u.OptionID equals o.ID
                           select new Options
                           {
                               ID = u.OptionID,
                               OptionName = o.OptionName,
                               OptionType = o.OptionType,
                               Icon = o.Icon,
                               URL = o.URL,
                               Enabled = o.Enabled,

                           }).ToList();

                #region CODIGO ANTERIOR
                /*using (SqlConnection conn = new SqlConnection(DBMain))
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
                }*/
                #endregion
            }
            catch (Exception ex)
            {
                options = null;
            }

            return options;
        }

        public static List<Options> GetOptionsByModule(string moduleID)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<Options> options = new List<Options>();

            try
            {
                options = db.Options.Where(o => o.ModuleID.ToString() == moduleID).ToList();

                #region CODIGO ANTERIOR
                //using (SqlConnection conn = new SqlConnection(DBMain))
                //{
                //    conn.Open();
                //    using (SqlCommand comm = new SqlCommand(string.Format("select * from Options where ModuleID = {0}", moduleID), conn))
                //    {
                //        using (SqlDataReader reader = comm.ExecuteReader())
                //        {
                //            while (reader.Read())
                //            {
                //                Option option = new Option();

                //                option.ID = reader["ID"].ToString();
                //                option.Name = reader["OptionName"].ToString();
                //                option.ModuleID = reader["ModuleID"].ToString();

                //                options.Add(option);
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                options = null;
            }

            return options;
        }
    }
}