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

            int days = 0;
            if (!client.SinCred)
            {
                days = Cond.GetCond(connect, client.Cond.ID).DaysCred;
            }

            query.Append("exec pInsertarCliente ");
            
            // GENERAL
            query.AppendFormat("@sCo_Cli = '{0}', ", client.ID);
            query.AppendFormat("@sCli_des = '{0}', ", client.Name);
            query.AppendFormat("@sTip_Cli = '{0}', ", client.Type.ID);
            query.AppendFormat("@sRif = '{0}', ", client.RIF);
            query.AppendFormat("@sdFecha_reg = '{0}', ", client.DateReg.ToString("MM-dd-yyyy"));
            query.AppendFormat("@bcontrib = '{0}', ", client.Contrib);
            query.AppendFormat("@binactivo = '{0}', ", client.Disabled);

            query.AppendFormat("@sCo_seg = '{0}', ", client.Segment.ID);
            query.AppendFormat("@sCo_Ven = '{0}', ", client.Seller.ID);
            query.AppendFormat("@sCo_Cta_Ingr_Egr = '{0}', ", client.Account.ID);
            query.AppendFormat("@srespons = {0}, ", StringController.VerifyValueDb(client.Respons));
            query.AppendFormat("@sTelefonos = {0}, ", StringController.VerifyValueDb(client.Phone));
            query.AppendFormat("@sfax = {0}, ", StringController.VerifyValueDb(client.Fax));
            query.AppendFormat("@semail = {0}, ", StringController.VerifyValueDb(client.Email));
            query.AppendFormat("@semail_alterno = {0}, ", StringController.VerifyValueDb(client.AltEmail));

            // UBICACION
            query.AppendFormat("@sCo_pais = '{0}', ", client.Country.ID);
            query.AppendFormat("@sCo_zon = '{0}', ", client.Zone.ID);
            query.AppendFormat("@sciudad = {0}, ", StringController.VerifyValueDb(client.City));
            query.AppendFormat("@sDirec1 = {0}, ", StringController.VerifyValueDb(client.Address1));
            query.AppendFormat("@sdir_ent2 = {0}, ", StringController.VerifyValueDb(client.DelAddress));

            // CREDITO
            query.AppendFormat("@bsincredito = '{0}', ", client.SinCred);
            query.AppendFormat("@sCond_Pag = {0}, ", StringController.VerifyValueDb(client.Cond.ID));
            query.AppendFormat("@iplaz_pag = {0}, ", days);
            query.AppendFormat("@demont_cre = {0}, ", client.MontCre);
            query.AppendFormat("@dedesc_ppago = {0}, ", client.DescPPago);
            query.AppendFormat("@dedesc_glob = {0}, ", client.DescGlob);
            query.AppendFormat("@scomentario = {0}, ", StringController.VerifyValueDb(client.Comment));

            // OTROS
            query.AppendFormat("@stipo_per = {0}, ", StringController.VerifyValueDb(client.TipPer));
            query.AppendFormat("@sCo_Tab = {0}, ", StringController.VerifyValueDb(client.CoTab));
            query.AppendFormat("@bcontribu_e = '{0}', ", client.ContribuE);
            query.AppendFormat("@bRete_Regis_Doc = '{0}', ", client.ReteRegisDoc);
            query.AppendFormat("@deporc_esp = {0}, ", client.PorcEsp);

            // ADICIONALES
            for (int i = 0; i < 8; i++)
            {
                query.AppendFormat("@scampo{0} = {1}, ", i + 1, StringController.VerifyValueDb(client.ExtraFields[i]));
            }

            query.Append("@sCo_Us_In = '', @sCo_Sucu_In = '', @sRevisado = null, @sTrasnfe = null, @iTipo_Adi = 1, @iPuntaje = 0, @iId = 0");

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

            int days = 0;
            if (!client.SinCred)
            {
                days = Cond.GetCond(connect, client.Cond.ID).DaysCred;
            }

            query.Append("UPDATE saCliente ");
            
            // GENERAL
            query.AppendFormat("SET cli_des = '{0}', ", client.Name);
            query.AppendFormat("tip_cli = '{0}', ", client.Type.ID);
            query.AppendFormat("rif = '{0}', ", client.RIF);
            query.AppendFormat("contrib = '{0}', ", client.Contrib);
            query.AppendFormat("inactivo = '{0}', ", client.Disabled);

            query.AppendFormat("co_seg = '{0}', ", client.Segment.ID);
            query.AppendFormat("co_ven = '{0}', ", client.Seller.ID);
            query.AppendFormat("co_cta_ingr_egr = '{0}', ", client.Account.ID);
            query.AppendFormat("respons = {0}, ", StringController.VerifyValueDb(client.Respons));
            query.AppendFormat("telefonos = {0}, ", StringController.VerifyValueDb(client.Phone));
            query.AppendFormat("fax = {0}, ", StringController.VerifyValueDb(client.Fax));
            query.AppendFormat("email = {0}, ", StringController.VerifyValueDb(client.Email));
            query.AppendFormat("email_alterno = {0}, ", StringController.VerifyValueDb(client.AltEmail));

            // UBICACION
            query.AppendFormat("co_pais = '{0}', ", client.Country.ID);
            query.AppendFormat("co_zon = '{0}', ", client.Zone.ID);
            query.AppendFormat("ciudad = {0}, ", StringController.VerifyValueDb(client.City));
            query.AppendFormat("direc1 = {0}, ", StringController.VerifyValueDb(client.Address1));
            query.AppendFormat("dir_ent2 = {0}, ", StringController.VerifyValueDb(client.DelAddress));

            // CREDITO
            query.AppendFormat("sincredito = '{0}'", client.SinCred);
            query.AppendFormat("cond_pag = {0}, ", StringController.VerifyValueDb(client.Cond.ID));
            query.AppendFormat("plaz_pag = {0}, ", days);
            query.AppendFormat("mont_cre = {0}, ", client.MontCre);
            query.AppendFormat("desc_ppago = {0}, ", client.DescPPago);
            query.AppendFormat("desc_glob = {0}, ", client.DescGlob);
            query.AppendFormat("comentario = {0}, ", StringController.VerifyValueDb(client.Comment));

            // OTROS
            query.AppendFormat("tipo_per = {0}, ", StringController.VerifyValueDb(client.TipPer));
            query.AppendFormat("co_tab = {0}, ", StringController.VerifyValueDb(client.CoTab));
            query.AppendFormat("contribu_e = '{0}', ", client.ContribuE);
            query.AppendFormat("rete_regis_doc = '{0}', ", client.ReteRegisDoc);
            query.AppendFormat("porc_esp = {0}, ", client.PorcEsp);

            // ADICIONALES
            for (int i = 0; i < 7; i++)
            {
                query.AppendFormat("campo{0} = {1}, ", i + 1, StringController.VerifyValueDb(client.ExtraFields[i]));
            }
            query.AppendFormat("campo8 = {0} ", StringController.VerifyValueDb(client.ExtraFields[7]));

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