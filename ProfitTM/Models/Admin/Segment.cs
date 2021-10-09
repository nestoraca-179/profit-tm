using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Segment
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static Segment GetSegment(string connect, string ID)
        {
            Segment segment;

            string query = string.Format("SELECT * FROM saSegmento WHERE co_seg = '{0}'", ID);
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                segment = new Segment()
                                {
                                    ID = reader["co_seg"].ToString().Trim(),
                                    Name = reader["seg_des"].ToString()
                                };
                            }
                            else
                            {
                                segment = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                segment = null;
            }

            return segment;
        }
    }
}