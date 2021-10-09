using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Option
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public char Type { get; set; }
        public string Function { get; set; }
        public List<Modal> Modals { get; set; }

        public static List<Modal> GetModals(string id)
        {
            List<Modal> modals = new List<Modal>();
            string DBadmin = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;

            string query = string.Format("select * from Modals where OptionID = '{0}'", id);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modals.Add(new Modal()
                                {
                                    ID = reader["ID"].ToString(),
                                    OptionID = reader["OptionID"].ToString(),
                                    Title = reader["ModalTitle"].ToString(),
                                    ModalType = Convert.ToChar(reader["ModalType"].ToString()),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                modals = null;
            }

            return modals;
        }
    }
}