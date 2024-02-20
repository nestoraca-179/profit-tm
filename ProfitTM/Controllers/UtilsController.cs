using Humanizer;
using System;
using System.Globalization;

namespace ProfitTM.Controllers
{
    public class UtilsController
    {
        public DateTime FormatDate(string date)
        {
            int anio = int.Parse(date.Split('-')[2]);
            int mes = int.Parse(date.Split('-')[1]);
            int dia = int.Parse(date.Split('-')[0]);

            DateTime fecha = new DateTime(anio, mes, dia);

            return fecha;
        }

        public string NumberToWords(decimal monto)
        {
            string number;

            int num_u = Convert.ToInt32(Math.Floor(monto));
            int dec_u = Convert.ToInt32((monto - num_u) * 100);

            if (dec_u > 0)
            {
                number = num_u.ToWords(new CultureInfo("es-ES")).ToUpper() + " CON " + dec_u + "/100 CENTIMOS";
            }
            else
            {
                number = num_u.ToWords(new CultureInfo("es-ES")).ToUpper();
            }

            return number;
        }
    }
}