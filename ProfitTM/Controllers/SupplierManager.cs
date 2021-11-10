using ProfitTM.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace ProfitTM.Controllers
{
    public class SupplierManager
    {
        public string connect { get; set; }
        public string DBadmin { get; set; }

        public SupplierManager()
        {
            this.connect = HttpContext.Current.Session["connect"].ToString();
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        public SupplierManager(string connect)
        {
            this.connect = connect;
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        public ProfitTMResponse addSupplier(Supplier supplier)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.Append("exec pInsertarProveedor ");
            query.Append("@sCo_Prov = '" + supplier.ID + "', ");
            query.Append("@sProv_des = '" + supplier.Name + "', ");
            query.Append("@sCo_seg = '" + supplier.Segment + "', ");
            query.Append("@sCo_zon = '" + supplier.Zone + "', ");
            query.Append("@sCo_pais = '" + supplier.Country + "', ");
            query.Append("@sdFecha_reg = '" + DateTime.Now.ToString("MM-dd-yyyy") + "', ");
            query.Append("@sTip_Pro = '" + supplier.Type + "', ");
            query.Append("@sRif = '" + supplier.RIF + "', ");
            query.Append("@sEmail = '" + supplier.Email + "', ");
            query.Append("@sTelefonos = '" + supplier.Phone + "', ");
            query.Append("@sDirec1 = '" + supplier.Address1 + "', ");
            query.Append("@sCo_Cta_Ingr_Egr = '" + supplier.Account + "', ");
            query.Append("@sCo_Tab = null, @bRete_Regis_Doc = 0, @sCo_Us_In = '', @sCo_Sucu_In = '', @sRevisado = null, @sTrasnfe = null, @iTipo_Adi = 1");

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

        public ProfitTMResponse editSupplier(Supplier supplier)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.Append("UPDATE saProveedor ");
            query.Append("SET prov_des = '" + supplier.Name + "', ");
            query.Append("rif = '" + supplier.RIF + "', ");
            query.Append("email = '" + supplier.Email + "', ");
            query.Append("telefonos = '" + supplier.Phone + "', ");
            query.Append("direc1 = '" + supplier.Address1 + "' ");
            query.Append("WHERE co_prov = '" + supplier.ID + "'");

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

        public ProfitTMResponse deleteSupplier(string ID)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            string query = string.Format("DELETE FROM saProveedor WHERE co_prov = '{0}'", ID);

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