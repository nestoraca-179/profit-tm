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

            query.Append("UPDATE saCliente ");
            query.Append("SET cli_des = '" + client.Name + "', ");
            query.Append("rif = '" + client.RIF + "', ");
            query.Append("email = '" + client.Email + "', ");
            query.Append("telefonos = '" + client.Phone + "', ");
            query.Append("direc1 = '" + client.Address1 + "' ");
            query.Append("WHERE co_cli = '" + client.ID + "'");

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