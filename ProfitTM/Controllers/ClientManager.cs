using ProfitTM.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace ProfitTM.Controllers
{
    public class ClientManager
    {
        public string connect { get; set; }
        public string DBadmin { get; set; }

        public ClientManager()
        {
            this.connect = HttpContext.Current.Session["connect"].ToString();
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        public ClientManager(string connect)
        {
            this.connect = connect;
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        public ProfitTMResponse addClient(Client client)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.Append("exec pInsertarCliente ");
            query.Append("@sCo_Cli = '" + client.ID + "', ");
            query.Append("@sCli_des = '" + client.Name + "', ");
            query.Append("@sCo_seg = '" + client.Segment + "', ");
            query.Append("@sCo_zon = '" + client.Zone + "', ");
            query.Append("@sCo_pais = '" + client.Country + "', ");
            query.Append("@sdFecha_reg = '" + DateTime.Now.ToString("MM-dd-yyyy") + "', ");
            query.Append("@sTip_Cli = '" + client.Type + "', ");
            query.Append("@sRif = '" + client.RIF + "', ");
            query.Append("@sEmail = '" + client.Email + "', ");
            query.Append("@sTelefonos = '" + client.Phone + "', ");
            query.Append("@sDirec1 = '" + client.Address1 + "', ");
            query.Append("@sCo_Cta_Ingr_Egr = '" + client.Account + "', ");
            query.Append("@sCo_Ven = '" + client.Seller + "', ");
            query.Append("@sCond_Pag = '" + client.Cond + "', ");
            query.Append("@sCo_Tab = null, @bRete_Regis_Doc = 0, @sCo_Us_In = '', @sCo_Sucu_In = '', @sRevisado = null, @sTrasnfe = null, @iTipo_Adi = 1, @iPuntaje = 0, ");
            query.Append("@iId = 0, @demont_cre = 0, @iplaz_pag = 30, @dedesc_ppago = 0, @dedesc_glob = 0, @deporc_esp = 0");

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        int rows = comm.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            response.Status = "OK";
                            response.Result = rows;
                        }
                        else
                        {
                            response.Status = "ERROR";
                            response.Message = "Se ha producido un error al ejecutar la sentencia SQL";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfitTMResponse editClient(Client client)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            Cond cond = Cond.GetCond(connect, client.Cond.ID);

            query.Append("UPDATE saCliente ");
            query.AppendFormat("SET cli_des = '{0}', ", client.Name);
            query.AppendFormat("rif = '{0}', ", client.RIF);
            query.AppendFormat("email = '{0}', ", client.Email);
            query.AppendFormat("telefonos = '{0}', ", client.Phone);
            query.AppendFormat("direc1 = '{0}', ", client.Address1);
            query.AppendFormat("co_seg = '{0}', ", client.Segment.ID);
            query.AppendFormat("co_zon = '{0}', ", client.Zone.ID);
            query.AppendFormat("tip_cli = '{0}', ", client.Type.ID);
            query.AppendFormat("contrib = '{0}', ", client.Contrib);
            query.AppendFormat("inactivo = '{0}', ", client.Disabled);
            query.AppendFormat("co_ven = '{0}', ", client.Seller.ID);
            query.AppendFormat("co_cta_ingr_egr = '{0}', ", client.Account.ID);
            query.AppendFormat("fax = {0}, ", !string.IsNullOrEmpty(client.Fax) ? ("'" + client.Fax + "'") : "NULL");
            query.AppendFormat("respons = {0}, ", !string.IsNullOrEmpty(client.Respons) ? ("'" + client.Respons + "'") : "NULL");
            query.AppendFormat("email_alterno = {0}, ", !string.IsNullOrEmpty(client.AltEmail) ? ("'" + client.AltEmail + "'") : "NULL");
            query.AppendFormat("co_pais = '{0}', ", client.Country.ID);
            query.AppendFormat("ciudad = {0}, ", !string.IsNullOrEmpty(client.City) ? ("'" + client.City + "'") : "NULL");
            query.AppendFormat("dir_ent2 = {0}, ", !string.IsNullOrEmpty(client.DelAddress) ? ("'" + client.DelAddress + "'") : "NULL");
            query.AppendFormat("cond_pag = '{0}', ", cond.ID);
            query.AppendFormat("plaz_pag = {0}, ", cond.DaysCred);
            query.AppendFormat("desc_ppago = {0}, ", client.DescPPago);
            query.AppendFormat("desc_glob = {0}, ", client.DescGlob);
            query.AppendFormat("comentario = {0}, ", !string.IsNullOrEmpty(client.Comment) ? ("'" + client.Comment + "'") : "NULL");
            query.AppendFormat("tipo_per = '{0}', ", client.TipPer);
            query.AppendFormat("co_tab = '{0}', ", client.CoTab);
            query.AppendFormat("contribu_e = '{0}', ", client.ContribuE);
            query.AppendFormat("porc_esp = {0}, ", client.PorcEsp);
            query.AppendFormat("rete_regis_doc = '{0}', ", client.ReteRegisDoc);

            for (int i = 0; i < 7; i++)
            {
                query.AppendFormat("campo{0} = {1}, ", i + 1, !string.IsNullOrEmpty(client.ExtraFields[i]) ? ("'" + client.ExtraFields[i] + "'") : "NULL");
            }

            query.AppendFormat("campo8 = {0} ", !string.IsNullOrEmpty(client.ExtraFields[7]) ? ("'" + client.ExtraFields[7] + "'") : "NULL");
            query.AppendFormat("WHERE co_cli = '{0}'", client.ID);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        int rows = comm.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            response.Status = "OK";
                            response.Result = rows;
                        }
                        else
                        {
                            response.Status = "ERROR";
                            response.Message = "Se ha producido un error al ejecutar la sentencia SQL";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfitTMResponse deleteClient(string ID)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            string query = string.Format("DELETE FROM saCliente WHERE co_cli = '{0}'", ID);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        int rows = comm.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            response.Status = "OK";
                            response.Result = rows;
                        }
                        else
                        {
                            response.Status = "ERROR";
                            response.Message = "Se ha producido un error al ejecutar la sentencia SQL";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }
    }
}