using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Transport : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static Transport GetTransport(string connect, string ID)
        //{
        //    Transport transport;

        //    string query = string.Format("SELECT * FROM saTransporte WHERE co_tran = '{0}'", ID);
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
        //                        transport = new Transport()
        //                        {
        //                            ID = reader["co_tran"].ToString().Trim(),
        //                            Name = reader["des_tran"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        transport = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        transport = null;
        //    }

        //    return transport;
        //}

        //public static List<Transport> GetAllTransports(string connect)
        //{
        //    List<Transport> transports = new List<Transport>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saTransporte", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        transports.Add(new Transport()
        //                        {
        //                            ID = reader["co_tran"].ToString(),
        //                            Name = reader["des_tran"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        transports = null;
        //    }

        //    return transports;
        //}
        #endregion

        public saTransporte GetTransportByID(string id)
        {
            saTransporte transport = new saTransporte();

            try
            {
                transport = db.saTransporte.SingleOrDefault(c => c.co_tran == id);
            }
            catch (Exception ex)
            {
                transport = null;
            }

            return transport;
        }

        public List<saTransporte> GetAllTransports()
        {
            List<saTransporte> transports = new List<saTransporte>();

            try
            {
                transports = db.saTransporte.ToList();
            }
            catch (Exception ex)
            {
                transports = null;
            }

            return transports;
        }
    }
}