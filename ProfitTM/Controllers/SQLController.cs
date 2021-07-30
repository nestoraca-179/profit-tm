using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfitTM.Models;
using System.Configuration;
using System.Text;

namespace ProfitTM.Controllers
{
    public class SQLController
    {
        public string connect { get; set; }
        public string DBadmin { get; set; }

        // CONSTRUCTORES

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

        // OPCIONES, REPORTES Y COMPLEMENTOS

        public ProfitTMResponse getReports(string prod, string mod)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<ReportTree> reportTrees = new List<ReportTree>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(string.Format("select * from TreeReports where Product = '{0}' and Module = '{1}'", prod, mod), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ReportTree reportTree = new ReportTree();

                                reportTree.ID = reader["ID"].ToString();
                                reportTree.Name = reader["NameTree"].ToString();
                                reportTree.Prod = reader["Product"].ToString();
                                reportTree.ReportGroups = new List<ReportGroup>();

                                reportTrees.Add(reportTree);
                            }
                        }
                    }

                    foreach (ReportTree reportTree in reportTrees)
                    {
                        using (SqlCommand comm = new SqlCommand(string.Format("select * from GroupReports where IDTree = '{0}'", reportTree.ID), conn))
                        {
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ReportGroup reportGroup = new ReportGroup();

                                    reportGroup.ID = reader["ID"].ToString();
                                    reportGroup.Name = reader["NameGroup"].ToString();
                                    reportGroup.Reports = new List<Report>();

                                    reportTree.ReportGroups.Add(reportGroup);
                                }
                            }
                        }

                        foreach (ReportGroup reportGroup in reportTree.ReportGroups)
                        {
                            using (SqlCommand comm = new SqlCommand(string.Format("select * from Reports where IDGroup = '{0}'", reportGroup.ID), conn))
                            {
                                using (SqlDataReader reader = comm.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Report report = new Report();

                                        report.ID = reader["ID"].ToString();
                                        report.Name = reader["NameReport"].ToString();
                                        report.Proc = reader["SProc"].ToString();
                                        report.Cols = reader["Cols"].ToString();
                                        report.Fields = reader["Fields"].ToString();
                                        report.Format = reader["FormatReport"].ToString();
                                        report.Params = reader["Params"].ToString();
                                        report.QueryParams = reader["QueryParams"].ToString();
                                        report.Enabled = bool.Parse(reader["IsEnabled"].ToString());

                                        reportGroup.Reports.Add(report);
                                    }
                                }
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = reportTrees;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

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

        public ProfitTMResponse getProds()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Product> results = new List<Product>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saArticulo", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Product()
                                {
                                    ID = reader["co_art"].ToString(),
                                    Name = reader["art_des"].ToString()
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

        public ProfitTMResponse getPrices()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Price> results = new List<Price>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saTipoPrecio", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Price()
                                {
                                    ID = reader["co_precio"].ToString(),
                                    Name = reader["des_precio"].ToString()
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

        public ProfitTMResponse getStorages()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Storage> results = new List<Storage>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saAlmacen", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Storage()
                                {
                                    ID = reader["co_alma"].ToString(),
                                    Name = reader["des_alma"].ToString()
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

        public ProfitTMResponse getClients()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Client> results = new List<Client>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saCliente", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Client()
                                {
                                    ID = reader["co_cli"].ToString(),
                                    Name = reader["cli_des"].ToString()
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

        public ProfitTMResponse getSuppliers()
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Supplier> results = new List<Supplier>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saProveedor", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Supplier()
                                {
                                    ID = reader["co_prov"].ToString(),
                                    Name = reader["prov_des"].ToString()
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

        // RESULTADOS DE LAS DIFERENTES OPCIONES

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

        public ProfitTMResponse getResultsReports(string proc, string cols, List<string> parameters, List<string> queryParams)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            StringBuilder query = new StringBuilder();
            List<string> columns = new List<string>(), deleted = new List<string>();

            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

            query.Append("exec " + proc);

            foreach (string str in cols.Split(','))
            {
                columns.Add(str);
            }

            if (parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    query.Append(" @" + queryParams[i] + "=\'" + parameters[i] + "\'");
                    if (i < parameters.Count - 1)
                        query.Append(",");
                }
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query.ToString(), conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, string> item = new Dictionary<string, string>();

                                foreach (string col in columns)
                                {
                                    if (!col.Contains("(") && !col.Contains(")"))
                                    {
                                        string key = col;

                                        if (key.Contains("$"))
                                        {
                                            key = key.Remove(key.Length - 1);
                                            deleted.Add(key);
                                        }

                                        if (key.Contains("#"))
                                        {
                                            key = key.Remove(key.Length - 1);

                                            double value_d = double.Parse(reader[key].ToString().Trim());
                                            item.Add(key, value_d.ToString("N2"));
                                        }
                                        else
                                        {
                                            item.Add(key, reader[key].ToString().Trim());
                                        }
                                    }
                                    else
                                    {
                                        int index = col.IndexOf("(");
                                        string nameField = col.Substring(0, index);

                                        string str = col.Substring(index + 1);
                                        str = str.Remove(str.Length - 1);

                                        string[] opers = str.Split(new string[] { "  " }, StringSplitOptions.None);
                                        
                                        List<double> resultsDouble = new List<double>();
                                        List<string> operators = new List<string>();

                                        foreach (string op in opers)
                                        {
                                            if (op == "+" || op == "-" || op == "*" || op == "/")
                                            {
                                                operators.Add(op);
                                            }
                                            else
                                            {
                                                double val = 0;
                                                if (double.TryParse(op, out val))
                                                {
                                                    resultsDouble.Add(val);
                                                }
                                                else
                                                {
                                                    resultsDouble.Add(double.Parse(item[op].ToString()));
                                                }
                                            }
                                        }

                                        double result = resultsDouble[0];

                                        for (int i = 0; i < operators.Count; i++)
                                        {
                                            switch (operators[i])
                                            {
                                                case "+":
                                                    result += resultsDouble[i + 1];
                                                    break;
                                                case "-":
                                                    result -= resultsDouble[i + 1];
                                                    break;
                                                case "*":
                                                    result *= resultsDouble[i + 1];
                                                    break;
                                                case "/":
                                                    result /= resultsDouble[i + 1];
                                                    break;
                                            }
                                        }

                                        item.Add(nameField, result.ToString("N2"));
                                    }
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

        // ESTADISTICAS DE LOS GRAFICOS DEL DASHBOARD ADMIN

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
