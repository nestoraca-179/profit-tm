using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Cond : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }
        //public int DaysCred { get; set; }

        //public static Cond GetCond(string connect, string ID)
        //{
        //    Cond cond;

        //    string query = string.Format("SELECT * FROM saCondicionPago WHERE co_cond = '{0}'", ID);
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
        //                        cond = new Cond()
        //                        {
        //                            ID = reader["co_cond"].ToString().Trim(),
        //                            Name = reader["cond_des"].ToString(),
        //                            DaysCred = int.Parse(reader["dias_cred"].ToString())
        //                        };
        //                    }
        //                    else
        //                    {
        //                        cond = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cond = null;
        //    }

        //    return cond;
        //}

        //public static List<Cond> GetAllConds(string connect)
        //{
        //    List<Cond> conds = new List<Cond>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saCondicionPago", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        conds.Add(new Cond()
        //                        {
        //                            ID = reader["co_cond"].ToString(),
        //                            Name = reader["cond_des"].ToString(),
        //                            DaysCred = int.Parse(reader["dias_cred"].ToString())
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        conds = null;
        //    }

        //    return conds;
        //}
        #endregion

        public saCondicionPago GetCondByID(string id)
        {
            saCondicionPago cond = new saCondicionPago();

            try
            {
                cond = db.saCondicionPago.SingleOrDefault(c => c.co_cond == id);
            }
            catch (Exception ex)
            {
                cond = null;
            }

            return cond;
        }

        public List<saCondicionPago> GetAllConds()
        {
            List<saCondicionPago> conds = new List<saCondicionPago>();

            try
            {
                conds = db.saCondicionPago.ToList();
            }
            catch (Exception ex)
            {
                conds = null;
            }

            return conds;
        }
    }
}