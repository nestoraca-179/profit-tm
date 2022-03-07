using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Bank : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static List<Bank> GetAllBanks(string connect)
        //{
        //    List<Bank> banks = new List<Bank>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saBanco", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        banks.Add(new Bank()
        //                        {
        //                            ID = reader["co_ban"].ToString(),
        //                            Name = reader["des_ban"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        banks = null;
        //    }

        //    return banks;
        //}
        #endregion

        public saBanco GetBankByID(string id)
        {
            saBanco bank = new saBanco();

            try
            {
                bank = db.saBanco.SingleOrDefault(c => c.co_ban == id);
            }
            catch (Exception ex)
            {
                bank = null;
            }

            return bank;
        }

        public List<saBanco> GetAllbanks()
        {
            List<saBanco> banks = new List<saBanco>();

            try
            {
                banks = db.saBanco.ToList();
            }
            catch (Exception ex)
            {
                banks = null;
            }

            return banks;
        }
    }
}