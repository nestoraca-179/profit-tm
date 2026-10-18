using ProfitTM.Models;
using System.Data.Entity.Core.EntityClient;

namespace ProfitTM.Controllers
{
    public class EntityController
    {
        public static EntityConnectionStringBuilder GetEntity(string conn)
        {
            EntityConnectionStringBuilder entity = new EntityConnectionStringBuilder();

            entity.Provider = "System.Data.SqlClient";
            entity.ProviderConnectionString = conn + ";MultipleActiveResultSets=True;App=EntityFramework;";
            entity.Metadata = @"res://*/Models.ProfitAdmModel.csdl|res://*/Models.ProfitAdmModel.ssdl|res://*/Models.ProfitAdmModel.msl";

            return entity;
        }

        public static string GetConnectionStringFromConn(Connections conn)
        {
            return string.Format("Server={0};Database={1};User Id={2};Password={3}", conn.Server, conn.DB, conn.Username, conn.Password); ;
        }
    }
}