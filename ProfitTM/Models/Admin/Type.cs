using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Type
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static Type GetTypeAdmin(string connect, string ID, string typePerson)
        {
            Type type;
            string table = "", field = "";
           
            switch (typePerson)
            {
                case "C":
                    table = "saTipoCliente";
                    field = "tip_cli";

                    break;
                case "P":
                    table = "saTipoProveedor";
                    field = "tip_pro";

                    break;
            }

            string query = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", table, field, ID);
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
                                type = new Type()
                                {
                                    ID = reader[field].ToString().Trim(),
                                    Name = reader["des_tipo"].ToString()
                                };
                            }
                            else
                            {
                                type = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                type = null;
            }

            return type;
        }
    }
}