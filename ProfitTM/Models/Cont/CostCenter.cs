using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class CostCenter
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static List<CostCenter> GetAllExpenseAccounts(string connect)
        {
            List<CostCenter> centers = new List<CostCenter>();
            string DBcont = connect;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBcont))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from scCentro", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                centers.Add(new CostCenter()
                                {
                                    ID = reader["co_cen"].ToString(),
                                    Name = reader["des_cen"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                centers = null;
                Incident.CreateIncident("ERROR BUSCANDO CENTROS DE COSTOS", ex);
            }

            return centers;
        }
    }
}