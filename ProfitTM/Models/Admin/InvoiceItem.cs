using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class InvoiceItem
    {
        public string Reng { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Storage Storage { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public string PriceCode { get; set; }
        public string ImpCode { get; set; }
        public string ImpPorc { get; set; }
        public double Amount { get; set; }
        public double IVA { get; set; }
        public string TypeArt { get; set; }

        public static string GetTypeItem(string connect, string ID)
        {
            string type = "";
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;
            string query = string.Format("select tipo from saArticulo where co_art = '{0}'", ID);

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
                                type = reader["tipo"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                type = "";
            }

            return type;
        }
    }
}