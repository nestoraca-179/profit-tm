using System;

namespace ProfitTM.Models
{
    public class Step : ProfitAdmManager
    {
        public static void CreateStep(string table, Guid guid, string user, string type, string fields)
        {
            saPista step = new saPista();

            step.fecha = DateTime.Now;
            step.tablaOri = table;
            step.rowguidOri = guid;
            step.usuario_id = user;
            step.tipo_op = type;
            step.campos = fields;
            step.rowguid = Guid.NewGuid();

            db.saPista.Add(step);
        }
    }
}