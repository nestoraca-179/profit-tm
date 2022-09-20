using ProfitTM.Controllers;
using System.Data.Entity.Core.EntityClient;
using System.Web;

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
    }
}