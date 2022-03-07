using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Segment : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static Segment GetSegment(string connect, string ID)
        //{
        //    Segment segment;

        //    string query = string.Format("SELECT * FROM saSegmento WHERE co_seg = '{0}'", ID);
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
        //                        segment = new Segment()
        //                        {
        //                            ID = reader["co_seg"].ToString().Trim(),
        //                            Name = reader["seg_des"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        segment = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        segment = null;
        //    }

        //    return segment;
        //}

        //public static List<Segment> GetAllSegments(string connect)
        //{
        //    List<Segment> segments = new List<Segment>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saSegmento", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        segments.Add(new Segment()
        //                        {
        //                            ID = reader["co_seg"].ToString(),
        //                            Name = reader["seg_des"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        segments = null;
        //    }

        //    return segments;
        //}
        #endregion

        public saSegmento GetSegmentByID(string id)
        {
            saSegmento segment = new saSegmento();

            try
            {
                segment = db.saSegmento.SingleOrDefault(s => s.co_seg == id);
            }
            catch (Exception ex)
            {
                segment = null;
            }

            return segment;
        }

        public List<saSegmento> GetAllSegments()
        {
            List<saSegmento> segments = new List<saSegmento>();

            try
            {
                segments = db.saSegmento.ToList();
            }
            catch (Exception ex)
            {
                segments = null;
            }

            return segments;
        }
    }
}