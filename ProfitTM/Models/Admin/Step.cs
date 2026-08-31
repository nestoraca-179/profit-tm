using System;

namespace ProfitTM.Models
{
    public class Step : ProfitAdmManager
    {
        public static void CreateStep(string table, Guid guid, string user, string sucur, string type, string fields)
        {
            try
            {
                saPista step = new saPista();

                step.fecha = DateTime.Now;
                step.tablaOri = table;
                step.rowguidOri = guid;
                step.usuario_id = user;
                step.co_sucu = sucur;
                step.tipo_op = type;
                step.campos = fields;
                step.maquina = "SERVER PROFIT WEB";
                step.rowguid = Guid.NewGuid();

                db.saPista.Add(step);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogsFact.CreateLogInFile($"STEP_ERROR - No se pudo registrar la pista: {ex.Message}");
                throw;
            }
        }
    }
}