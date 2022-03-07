using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Storage : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static Storage GetStorage(string connect, string ID)
        //{
        //    Storage storage;

        //    string query = string.Format("SELECT * FROM saAlmacen WHERE co_alma = '{0}'", ID);
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
        //                        storage = new Storage()
        //                        {
        //                            ID = reader["co_alma"].ToString().Trim(),
        //                            Name = reader["des_alma"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        storage = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        storage = null;
        //    }

        //    return storage;
        //}

        //public static List<Storage> GetAllStorages(string connect)
        //{
        //    List<Storage> storages = new List<Storage>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saAlmacen", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        storages.Add(new Storage()
        //                        {
        //                            ID = reader["co_alma"].ToString(),
        //                            Name = reader["des_alma"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        storages = null;
        //    }

        //    return storages;
        //}
        #endregion

        public saAlmacen GetAlmByID(string id)
        {
            saAlmacen alm = new saAlmacen();

            try
            {
                alm = db.saAlmacen.SingleOrDefault(c => c.co_alma == id);
            }
            catch (Exception ex)
            {
                alm = null;
            }

            return alm;
        }

        public List<saAlmacen> GetAllAlms()
        {
            List<saAlmacen> alms = new List<saAlmacen>();

            try
            {
                alms = db.saAlmacen.ToList();
            }
            catch (Exception ex)
            {
                alms = null;
            }

            return alms;
        }
    }
}