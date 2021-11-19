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
        public string Model { get; set; }
        public Storage Storage { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public string PriceCode { get; set; }
        public string TipoImp { get; set; }
        public string TipoImp2 { get; set; }
        public string TipoImp3 { get; set; }
        public double PorcImp { get; set; }
        public double PorcImp2 { get; set; }
        public double PorcImp3 { get; set; }
        public double MontImp { get; set; }
        public double MontImp2 { get; set; }
        public double MontImp3 { get; set; }
        public double Amount { get; set; }
        public double IVA { get; set; }
        public string PorcDesc { get; set; }
        public double MontDesc { get; set; }
        public string TypeArt { get; set; }
        public string Rowguid { get; set; }
        public double Pend { get; set; }
        public double Pend2 { get; set; }
        public string TipDoc { get; set; }
        public string NumDoc { get; set; }
        public string RowguidDoc { get; set; }
        public double TotDev { get; set; }
        public double MontDev { get; set; }
        public double Others { get; set; }
        public string Comment { get; set; }
        public bool LotAsign { get; set; }
        public double MontDescGlob { get; set; }
        public double MontRecaGlob { get; set; }
        public double Others1Glob { get; set; }
        public double Others2Glob { get; set; }
        public double Others3Glob { get; set; }
        public double MontImpAfecGlob { get; set; }
        public double MontImp2AfecGlob { get; set; }
        public double MontImp3AfecGlob { get; set; }
        public string DisCen { get; set; }

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