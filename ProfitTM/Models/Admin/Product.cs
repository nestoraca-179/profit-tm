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
            saArticulo art;

            try
            {
                art = db.saArticulo.AsNoTracking().SingleOrDefault(c => c.co_art == id);
            }
            catch (Exception ex)
            {
                art = null;
                Incident.CreateIncident("ERROR BUSCANDO ARTICULO " + id, ex);
            }

            return art;
        }

        public List<saArticulo> GetAllArts()
        {
            List<saArticulo> arts;

            try
            {
                arts = db.saArticulo.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                arts = null;
                Incident.CreateIncident("ERROR BUSCANDO ARTICULOS", ex);
            }

            return arts;
        }

        public List<string> GetAllNameSellArts()
        {
            List<string> prods;

            try
            {
                prods = (from a in db.saArticulo.AsNoTracking()
                         where a.co_art.Trim().Substring(0, 1) == "4"
                         select a.co_art.Trim() + "/" + a.art_des.Trim()).ToList();
            }
            catch (Exception ex)
            {
                prods = null;
                Incident.CreateIncident("ERROR BUSCANDO NOMBRES DE PRODUCTOS DE VENTA", ex);
            }

            return prods;
        }

        public List<saArticulo> GetMostProducts(DateTime fec_d, DateTime fec_h, int number, bool selling)
        {
            List<saArticulo> prods = new List<saArticulo>();

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

                    prods.Add(articulo);
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

                    prods.Add(articulo);
                }
            }

            prods = (from a in prods
                    group decimal.Parse(a.campo1) by (a.co_art, a.art_des) into g
                    select new saArticulo
                    {
                        co_art = g.Key.co_art,
                        art_des = g.Key.art_des,
                        campo1 = Math.Round(g.Sum(), 2).ToString()

                    }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

            if (prods.Count > number)
                prods.RemoveRange(number, prods.Count - number);

            return prods;
        }
    }
}