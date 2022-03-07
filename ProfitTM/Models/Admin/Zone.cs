using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Zone : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static Zone GetZone(string connect, string ID)
        //{
        //    Zone zone;

        //    string query = string.Format("SELECT * FROM saZona WHERE co_zon = '{0}'", ID);
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
        //                        zone = new Zone()
        //                        {
        //                            ID = reader["co_zon"].ToString().Trim(),
        //                            Name = reader["zon_des"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        zone = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        zone = null;
        //    }

        //    return zone;
        //}

        //public static List<Zone> GetAllZones(string connect)
        //{
        //    List<Zone> zones = new List<Zone>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saZona", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        zones.Add(new Zone()
        //                        {
        //                            ID = reader["co_zon"].ToString(),
        //                            Name = reader["zon_des"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        zones = null;
        //    }

        //    return zones;
        //}
        #endregion

        public saZona GetZoneByID(string id)
        {
            saZona zone = new saZona();

            try
            {
                zone = db.saZona.SingleOrDefault(z => z.co_zon == id);
            }
            catch (Exception ex)
            {
                zone = null;
            }

            return zone;
        }

        public List<saZona> GetAllZones()
        {
            List<saZona> zones = new List<saZona>();

            try
            {
                zones = db.saZona.ToList();
            }
            catch (Exception ex)
            {
                zones = null;
            }

            return zones;
        }
    }
}