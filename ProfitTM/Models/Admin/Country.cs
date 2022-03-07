using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Country : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }

        //public static Country GetCountry(string connect, string ID)
        //{
        //    Country country;

        //    string query = string.Format("SELECT * FROM saPais WHERE co_pais = '{0}'", ID);
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
        //                        country = new Country()
        //                        {
        //                            ID = reader["co_pais"].ToString().Trim(),
        //                            Name = reader["pais_des"].ToString()
        //                        };
        //                    }
        //                    else
        //                    {
        //                        country = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        country = null;
        //    }

        //    return country;
        //}

        //public static List<Country> GetAllCountries(string connect)
        //{
        //    List<Country> countries = new List<Country>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saPais", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        countries.Add(new Country()
        //                        {
        //                            ID = reader["co_pais"].ToString(),
        //                            Name = reader["pais_des"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        countries = null;
        //    }

        //    return countries;
        //}
        #endregion

        public saPais GetCountryByID(string id)
        {
            saPais country = new saPais();

            try
            {
                country = db.saPais.SingleOrDefault(c => c.co_pais == id);
            }
            catch (Exception ex)
            {
                country = null;
            }

            return country;
        }

        public List<saPais> GetAllCountries()
        {
            List<saPais> countrys = new List<saPais>();

            try
            {
                countrys = db.saPais.ToList();
            }
            catch (Exception ex)
            {
                countrys = null;
            }

            return countrys;
        }
    }
}