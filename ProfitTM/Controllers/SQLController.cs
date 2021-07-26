using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfitTM.Models;
using System.Configuration;
using Http = System.Web.Http;

namespace ProfitTM.Controllers
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

        public SQLController(string connect)
        {
            this.connect = connect;
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        [System.Web.Mvc.NonAction]
        public ProfitTMResponse getOptions(string prod)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Option> options = new List<Option>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(string.Format("select * from Options where Product = '{0}'", prod), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Option option = new Option();

                                option.ID = reader["ID"].ToString();
                                option.Name = reader["NameOption"].ToString();
                                option.Icon = reader["Icon"].ToString();
                                option.Product = reader["Product"].ToString();
                                option.UrlTable = reader["UrlTable"].ToString();
                                option.UrlProcess = reader["UrlProcess"].ToString();
                                option.UrlReport = reader["UrlReport"].ToString();

                                options.Add(option);
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = options;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [System.Web.Mvc.NonAction]
        public ProfitTMResponse getMostSelledProducts(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Product> products = new List<Product>();

            int i = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(@"
                    select F.co_art, A.art_des, SUM(F.total_art) as total, SUM(reng_neto) as monto
                    from saFacturaVentaReng F
                    inner
                    join saArticulo A on F.co_art = A.co_art
                    group by F.co_art, A.art_des
                    order by total desc", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read() && i < number)
                            {
                                products.Add(new Product()
                                {
                                    ID = reader["co_art"].ToString().Trim(),
                                    Name = reader["art_des"].ToString().Trim(),
                                    Stock = double.Parse(reader["total"].ToString()),
                                    Price = double.Parse(reader["monto"].ToString())
                                });

                                i++;
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = products;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [System.Web.Mvc.NonAction]
        public ProfitTMResponse getMostPurchasedProducts(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Product> products = new List<Product>();

            int i = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(@"
                    select F.co_art, A.art_des, SUM(F.total_art) as total, SUM(reng_neto) as monto
                    from saFacturaCompraReng F
                    inner
                    join saArticulo A on F.co_art = A.co_art
                    group by F.co_art, A.art_des
                    order by total desc", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read() && i < number)
                            {
                                products.Add(new Product()
                                {
                                    ID = reader["co_art"].ToString().Trim(),
                                    Name = reader["art_des"].ToString().Trim(),
                                    Stock = double.Parse(reader["total"].ToString()),
                                    Price = double.Parse(reader["monto"].ToString())
                                });

                                i++;
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = products;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [System.Web.Mvc.NonAction]
        public ProfitTMResponse getMostActiveClients(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Client> clients = new List<Client>();

            int i = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("exec RepClienteMasVenta", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read() && i < number)
                            {
                                clients.Add(new Client()
                                {
                                    ID = reader["co_cli"].ToString(),
                                    Name = reader["cli_des"].ToString(),
                                    Amount = double.Parse(reader["Venta"].ToString())
                                });

                                i++;
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = clients;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [System.Web.Mvc.NonAction]
        public ProfitTMResponse getMostMorousClients(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Client> clients = new List<Client>();

            int i = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(@"CREATE TABLE #TempTable(
	                    descrip char(6),
	                    cob_num char(20),
	                    nro_doc char(20),
	                    fec_emis smalldatetime,
	                    fec_venc smalldatetime,
	                    co_tipo_doc char(6),
	                    total_neto decimal(18, 2),
	                    MONTO decimal(18, 2),
	                    saldo decimal(18, 2),
	                    nro_fact char(6),
	                    nro_orig varchar(20),
	                    tot_debe decimal(18, 2),
	                    tot_haber decimal(18, 2),
	                    co_prov char(16),
	                    prov_des varchar(100),
	                    co_mone char(6),
	                    anulado bit,
	                    ORIG char(30),
	                    observa varchar(max),
	                    n_pago char(6),
	                    PROV char(16),
	                    SaldoInic decimal(18, 2),
	                    SaldoFinal decimal(18, 2),
	                    tipo_rep varchar(20)
                    )

                    INSERT INTO #TempTable exec RepEstadoCuentaCli

                    SELECT DC.co_prov, C.cli_des, (SUM(DC.tot_debe) - SUM(DC.tot_haber)) as saldo FROM #TempTable DC
                    INNER JOIN saCliente C ON DC.co_prov collate Modern_Spanish_CI_AS = C.co_cli
                    GROUP BY DC.co_prov, C.cli_des
                    ORDER BY saldo DESC

                    DROP TABLE #TempTable", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read() && i < number)
                            {
                                clients.Add(new Client()
                                {
                                    ID = reader["co_prov"].ToString(),
                                    Name = reader["cli_des"].ToString(),
                                    Amount = double.Parse(reader["saldo"].ToString())
                                });

                                i++;
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = clients;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [System.Web.Mvc.NonAction]
        public ProfitTMResponse getMostActiveSuppliers(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Supplier> suppliers = new List<Supplier>();

            int i = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("exec RepProveedorMasCompra", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read() && i < number)
                            {
                                suppliers.Add(new Supplier()
                                {
                                    ID = reader["co_prov"].ToString(),
                                    Name = reader["prov_des"].ToString(),
                                    Amount = double.Parse(reader["Compra"].ToString())
                                });

                                i++;
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = suppliers;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [System.Web.Mvc.NonAction]
        public ProfitTMResponse getMostMorousSuppliers(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Supplier> suppliers = new List<Supplier>();

            int i = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(@"CREATE TABLE #TempTable(
	                    descrip char(6),
	                    cob_num char(20),
	                    nro_doc char(20),
	                    fec_emis smalldatetime,
	                    fec_venc smalldatetime,
	                    co_tipo_doc char(6),
	                    total_neto decimal(18, 2),
	                    MONTO decimal(18, 2),
	                    saldo decimal(18, 2),
	                    nro_fact varchar(20),
	                    nro_orig varchar(20),
	                    tot_debe decimal(18, 2),
	                    tot_haber decimal(18, 2),
	                    co_prov char(16),
	                    prov_des varchar(max),
	                    co_mone char(6),
	                    anulado bit,
	                    ORIG char(30),
	                    observa varchar(max),
	                    n_pago char(6),
	                    PROV char(16),
	                    SaldoInic decimal(18, 2),
	                    SaldoFinal decimal(18, 2),
	                    tipo_rep varchar(20)
                    )

                    INSERT INTO #TempTable exec RepEstadoCuentaProv

                    SELECT DC.co_prov, P.prov_des, (SUM(DC.tot_debe) - SUM(DC.tot_haber)) as saldo FROM #TempTable DC
                    INNER JOIN saProveedor P ON DC.co_prov collate Modern_Spanish_CI_AS = P.co_prov
                    GROUP BY DC.co_prov, P.prov_des
                    ORDER BY saldo DESC

                    DROP TABLE #TempTable", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read() && i < number)
                            {
                                suppliers.Add(new Supplier()
                                {
                                    ID = reader["co_prov"].ToString(),
                                    Name = reader["prov_des"].ToString(),
                                    Amount = double.Parse(reader["saldo"].ToString())
                                });

                                i++;
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = suppliers;
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
