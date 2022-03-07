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
        private static EntityConnectionStringBuilder entity;
        
        // CONTEXTO EF
        public static ProfitAdmEntities db;

        public ProfitAdmManager()
        {
            connect = HttpContext.Current.Session["connect"].ToString();
            entity = EntityController.GetEntity(connect);
            db = new ProfitAdmEntities(entity.ToString());
        }
    }
}