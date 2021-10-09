using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Supplier : Person
    {
        public static Supplier GetSupplier(string connect, string ID)
        {
            Supplier supplier;

            string query = string.Format("SELECT * FROM saProveedor WHERE co_prov = '{0}'", ID);
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
                                supplier = new Supplier()
                                {
                                    ID = reader["co_prov"].ToString().Trim(),
                                    Name = reader["prov_des"].ToString(),
                                    Type = Type.GetTypeAdmin(connect, reader["tip_pro"].ToString(), "P"),
                                    Zone = Zone.GetZone(connect, reader["co_zon"].ToString()),
                                    Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
                                    Country = Country.GetCountry(connect, reader["co_pais"].ToString()),
                                    Segment = Segment.GetSegment(connect, reader["co_seg"].ToString()),
                                    RIF = reader["rif"].ToString(),
                                    Email = reader["telefonos"].ToString(),
                                    Phone = reader["email"].ToString(),
                                    Address = reader["direc1"].ToString()
                                };
                            }
                            else
                            {
                                supplier = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                supplier = null;
            }

            return supplier;
        }
    }
}