using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfitTM.Models
{
    public class Product : ProfitAdmManager
    {
        public string co_art { get; set; }
        public string art_des { get; set; }

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

        public List<string> GetAllNameSellArts()
        {
            List<string> prods = new List<string>();

            try
            {
                prods = (from a in db.saArticulo.AsNoTracking()
                         where a.co_art.Trim().Substring(0, 1) == "4"
                         select a.co_art.Trim() + "/" + a.art_des.Trim()).ToList();
            }
            catch (Exception ex)
            {
                prods = null;
            }

            return prods;
        }

        public List<saArticulo> GetMostProducts(DateTime fec_d, DateTime fec_h, int number, bool selling)
        {
            List<saArticulo> articulos = new List<saArticulo>();

            #region CODIGO ANTERIOR
            //if (selling)
            //{
            //    articulos = (from r in rengsV
            //                 join a in arts on r.co_art equals a.co_art
            //                 where r.fe_us_in >= fec_d && r.fe_us_in <= fec_h
            //                 group r.total_art by (r.co_art, a.art_des) into g
            //                 select new saArticulo
            //                 {
            //                     co_art = g.Key.co_art,
            //                     art_des = g.Key.art_des,
            //                     campo1 = Math.Round(g.Sum(), 2).ToString()

            //                 }).OrderByDescending(x => double.Parse(x.campo1)).ToList();
            //}
            //else
            //{
            //    articulos = (from r in rengsC
            //                 join a in arts on r.co_art equals a.co_art
            //                 where r.fe_us_in >= fec_d && r.fe_us_in <= fec_h
            //                 group r.total_art by (r.co_art, a.art_des) into g
            //                 select new saArticulo
            //                 {
            //                     co_art = g.Key.co_art,
            //                     art_des = g.Key.art_des,
            //                     campo1 = Math.Round(g.Sum(), 2).ToString()

            //                 }).OrderByDescending(x => double.Parse(x.campo1)).ToList();
            //}
            #endregion

            if (selling) // ARTICULOS VENTAS
            {
                var sp = db.RepTotalVentaxArticulo(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
                var enumerator = sp.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    saArticulo articulo = new saArticulo();

                    articulo.co_art = enumerator.Current.co_art.Trim();
                    articulo.art_des = enumerator.Current.art_des.Trim();
                    articulo.campo1 = (enumerator.Current.total_art - enumerator.Current.total_dev).ToString();

                    articulos.Add(articulo);
                }
            }
            else // ARTICULOS COMPRAS
            {
                var sp = db.RepTotalCompraxArticulo(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
                var enumerator = sp.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    saArticulo articulo = new saArticulo();

                    articulo.co_art = enumerator.Current.co_art.Trim();
                    articulo.art_des = enumerator.Current.art_des.Trim();
                    articulo.campo1 = (enumerator.Current.total_art - enumerator.Current.total_dev).ToString();

                    articulos.Add(articulo);
                }
            }

            articulos = (from a in articulos
                         group decimal.Parse(a.campo1) by (a.co_art, a.art_des) into g
                         select new saArticulo
                         {
                             co_art = g.Key.co_art,
                             art_des = g.Key.art_des,
                             campo1 = Math.Round(g.Sum(), 2).ToString()

                         }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

            if (articulos.Count > number)
                articulos.RemoveRange(number, articulos.Count - number);

            return articulos;
        }
    }
}