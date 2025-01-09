using System.Web;
using ProfitTM.Controllers;
using System.Data.Entity.Core.EntityClient;

namespace ProfitTM.Models
{
    public abstract class ProfitAdmManager
    {
        // CADENA DE CONEXION
        private static string connect;

        // ENTIDAD PARA EF
        public static EntityConnectionStringBuilder entity;
        
        // CONTEXTO EF
        public static ProfitAdmEntities db;

        public ProfitAdmManager()
        {
            connect = HttpContext.Current.Session["CONNECT"].ToString();
            entity = EntityController.GetEntity(connect);
            db = new ProfitAdmEntities(entity.ToString());
        }

        public string GetNextConsec(ProfitAdmEntities context, string sucur, string serie)
        {
            string num = "";

            var sp = context.pConsecutivoProximo(sucur, serie).GetEnumerator();
            if (sp.MoveNext())
                num = sp.Current;

            sp.Dispose();
            return num;
        }
    }
}