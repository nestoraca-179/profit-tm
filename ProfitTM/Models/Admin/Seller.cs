using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Seller : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }
        //public char Type { get; set; }
        //public string Number { get; set; }
        //public string Phone { get; set; }
        //public string Address { get; set; }

        //public static Seller GetSeller(string connect, string ID)
        //{
        //    Seller seller;

        //    string query = string.Format("SELECT * FROM saVendedor WHERE co_ven = '{0}'", ID);
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
        //                        seller = new Seller()
        //                        {
        //                            ID = reader["co_ven"].ToString().Trim(),
        //                            Name = reader["ven_des"].ToString(),
        //                            Type = Convert.ToChar(reader["tipo"].ToString().Trim()),
        //                            Number = reader["cedula"].ToString().Trim(),
        //                            Phone = reader["telefonos"].ToString(),
        //                            Address = reader["direc1"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        seller = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        seller = null;
        //    }

        //    return seller;
        //}

        //public static List<Seller> GetAllSellers(string connect)
        //{
        //    List<Seller> sellers = new List<Seller>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saVendedor", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        sellers.Add(new Seller()
        //                        {
        //                            ID = reader["co_ven"].ToString(),
        //                            Name = reader["ven_des"].ToString(),
        //                            Type = Convert.ToChar(reader["tipo"].ToString().Trim()),
        //                            Number = reader["cedula"].ToString().Trim(),
        //                            Phone = reader["telefonos"].ToString(),
        //                            Address = reader["direc1"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sellers = null;
        //    }

        //    return sellers;
        //}
        #endregion

        public saVendedor GetSellerByID(string id)
        {
            saVendedor seller = new saVendedor();

            try
            {
                seller = db.saVendedor.SingleOrDefault(s => s.co_ven == id);
            }
            catch (Exception ex)
            {
                seller = null;
            }

            return seller;
        }

        public List<saVendedor> GetAllSellers()
        {
            List<saVendedor> sellers = new List<saVendedor>();

            try
            {
                sellers = db.saVendedor.ToList();
            }
            catch (Exception ex)
            {
                sellers = null;
            }

            return sellers;
        }
    }
}