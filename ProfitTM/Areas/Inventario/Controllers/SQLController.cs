using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using ProfitTM.Models;
using Http = System.Web.Http;

namespace ProfitTM.Areas.Inventario.Controllers
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

        [NonAction]
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

        [NonAction]
        public ProfitTMResponse getResultsReports(string proc, string cols, List<string> parameters, List<string> queryParams)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            StringBuilder query = new StringBuilder();
            List<string> columns = new List<string>();

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
                                        item.Add(col, reader[col].ToString().Trim());
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

                                        item.Add(nameField, result.ToString());
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

        [NonAction]
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
                                results.Add(new Product() {
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

        [NonAction]
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

        [NonAction]
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
    }
}