using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Http = System.Web.Http;

namespace ProfitTM.Areas.Compras.Controllers
{
    public class SQLController : Http.ApiController
    {
        public string connect { get; set; }
        public string DBadmin { get; set; }

        public SQLController()
        {
            this.connect = HttpContext.Current.Session["connect"].ToString();
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        [NonAction]
        public ProfitTMResponse getResultsTable(string cols, string table)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            List<string> columns = new List<string>();
            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
            
            string query = "select " + cols + " from " + table;

            foreach (string str in cols.Split(','))
            {
                columns.Add(str);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, string> item = new Dictionary<string, string>();

                                foreach (string col in columns)
                                {
                                    item.Add(col, reader[col].ToString());
                                }

                                results.Add(item);
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = results;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [NonAction]
        public ProfitTMResponse getTypes()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Models.Type> results = new List<Models.Type>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saTipoProveedor", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Models.Type()
                                {
                                    ID = reader["tip_pro"].ToString(),
                                    Name = reader["des_tipo"].ToString()
                                });
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = results;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [NonAction]
        public ProfitTMResponse getZones()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Zone> results = new List<Zone>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saZona", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Zone()
                                {
                                    ID = reader["co_zon"].ToString(),
                                    Name = reader["zon_des"].ToString()
                                });
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = results;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [NonAction]
        public ProfitTMResponse getAccounts()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Account> results = new List<Account>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saCuentaIngEgr", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Account()
                                {
                                    ID = reader["co_cta_ingr_egr"].ToString(),
                                    Name = reader["descrip"].ToString()
                                });
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = results;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [NonAction]
        public ProfitTMResponse getCountries()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Country> results = new List<Country>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saPais", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Country()
                                {
                                    ID = reader["co_pais"].ToString(),
                                    Name = reader["pais_des"].ToString()
                                });
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = results;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [NonAction]
        public ProfitTMResponse getSegments()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Segment> results = new List<Segment>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saSegmento", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Segment()
                                {
                                    ID = reader["co_seg"].ToString(),
                                    Name = reader["seg_des"].ToString()
                                });
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = results;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [NonAction]
        public ProfitTMResponse addSupplier(Supplier supplier)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.Append("exec pInsertarProveedor ");
            query.Append("@sCo_Prov = '" + supplier.ID +  "', ");
            query.Append("@sProv_des = '" + supplier.Name + "', ");
            query.Append("@sCo_seg = '" + supplier.Segment + "', ");
            query.Append("@sCo_zon = '" + supplier.Zone + "', ");
            query.Append("@sCo_pais = '" + supplier.Country + "', ");
            query.Append("@sdFecha_reg = '" + DateTime.Now.ToString("MM-dd-yyyy") + "', ");
            query.Append("@sTip_Pro = '" + supplier.Type + "', ");
            query.Append("@sRif = '" + supplier.RIF + "', ");
            query.Append("@sNit = '" + supplier.NIT + "', ");
            query.Append("@sEmail = '" + supplier.Email + "', ");
            query.Append("@sTelefonos = '" + supplier.Phone + "', ");
            query.Append("@sDirec1 = '" + supplier.Address + "', ");
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

        [NonAction]
        public ProfitTMResponse editSupplier(Supplier supplier)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            query.Append("UPDATE saProveedor ");
            query.Append("SET prov_des = '" + supplier.Name + "', ");
            query.Append("rif = '" + supplier.RIF + "', ");
            query.Append("email = '" + supplier.Email + "', ");
            query.Append("telefonos = '" + supplier.Phone + "', ");
            query.Append("direc1 = '" + supplier.Address + "' ");
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

        [NonAction]
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