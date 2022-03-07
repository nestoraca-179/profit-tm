using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Product : ProfitAdmManager
    {
        #region CODIGO ANTERIOR
        //public string ID { get; set; }
        //public string Name { get; set; }
        //public double Stock { get; set; }
        //public double Price { get; set; }

        //public static List<Product> GetAllProducts(string connect)
        //{
        //    List<Product> products = new List<Product>();
        //    string DBadmin = connect;

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(DBadmin))
        //        {
        //            conn.Open();
        //            using (SqlCommand comm = new SqlCommand("select * from saArticulo", conn))
        //            {
        //                using (SqlDataReader reader = comm.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        products.Add(new Product()
        //                        {
        //                            ID = reader["co_art"].ToString(),
        //                            Name = reader["art_des"].ToString()
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        products = null;
        //    }

        //    return products;
        //}
        #endregion

        public saArticulo GetArtByID(string id)
        {
            saArticulo art = new saArticulo();

            try
            {
                art = db.saArticulo.SingleOrDefault(c => c.co_art == id);
            }
            catch (Exception ex)
            {
                art = null;
            }

            return art;
        }

        public List<saArticulo> GetAllArts()
        {
            List<saArticulo> arts = new List<saArticulo>();

            try
            {
                arts = db.saArticulo.ToList();
            }
            catch (Exception ex)
            {
                arts = null;
            }

            return arts;
        }

        public ProfitTMResponse GetMostProducts(int number, bool selling)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<saArticulo> articulos = new List<saArticulo>();

            try
            {
                List<saFacturaVentaReng> rengsV = db.saFacturaVentaReng.ToList();
                List<saFacturaCompraReng> rengsC = db.saFacturaCompraReng.ToList();
                List<saArticulo> arts = db.saArticulo.ToList();

                if (selling)
                {
                    articulos = (from r in rengsV
                                 join a in arts on r.co_art equals a.co_art
                                 group r.total_art by (r.co_art, a.art_des) into g
                                 select new saArticulo
                                 {
                                     co_art = g.Key.co_art,
                                     art_des = g.Key.art_des,
                                     campo1 = Math.Round(g.Sum(), 2).ToString()

                                 }).OrderByDescending(x => double.Parse(x.campo1)).ToList();
                }
                else
                {
                    articulos = (from r in rengsC
                                 join a in arts on r.co_art equals a.co_art
                                 group r.total_art by (r.co_art, a.art_des) into g
                                 select new saArticulo
                                 {
                                     co_art = g.Key.co_art,
                                     art_des = g.Key.art_des,
                                     campo1 = Math.Round(g.Sum(), 2).ToString()

                                 }).OrderByDescending(x => double.Parse(x.campo1)).ToList();
                }

                if (articulos.Count > number)
                    articulos.RemoveRange(number, articulos.Count - number);

                response.Status = "OK";
                response.Result = articulos;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = "MP - " + ex.Message;
            }

            return response;
        }
    }
}