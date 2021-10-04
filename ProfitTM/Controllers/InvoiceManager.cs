using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ProfitTM.Controllers
{
    public class InvoiceManager
    {
        public string connect { get; set; }
        public string DBadmin { get; set; }

        public InvoiceManager()
        {
            this.connect = HttpContext.Current.Session["connect"].ToString();
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        public InvoiceManager(string connect)
        {
            this.connect = connect;
            this.DBadmin = ConfigurationManager.ConnectionStrings[this.connect].ConnectionString;
        }

        private ProfitTMResponse getInvoiceDetails(string id, string type)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            Dictionary<string, string> details = new Dictionary<string, string>();

            string query = "";

            if (type == "V")
            {
                query = @"SELECT TOP (1) 
                [Extent1].[co_cli] AS id,
                [Extent1].[cli_des] AS descrip, 
                [Extent1].[cond_pag] AS cond,
                [Extent1].[co_ven] AS vend
                FROM (SELECT 
                      [v_saCliente_saTipoCliente].[co_cli] AS [co_cli],
                      [v_saCliente_saTipoCliente].[cli_des] AS [cli_des], 
                      [v_saCliente_saTipoCliente].[cond_pag] AS [cond_pag],
                      [v_saCliente_saTipoCliente].[co_ven] AS [co_ven]
                      FROM [dbo].[v_saCliente_saTipoCliente] AS [v_saCliente_saTipoCliente]) AS [Extent1]
                WHERE [Extent1].[co_cli] = N'" + id + "'";
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
                            if (reader.Read())
                            {
                                details.Add("cond", reader["cond"].ToString().Trim());
                                details.Add("vend", reader["vend"].ToString().Trim());

                                response.Status = "OK";
                                response.Result = details;
                            }
                            else
                            {
                                response.Status = "ERROR";
                                response.Message = "No se encontraron detalles del cliente " + id;
                            }
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

        public ProfitTMResponse getAllInvoices(string type)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<Invoice> invoices = new List<Invoice>();

            string name = "", table = "";

            switch (type)
            {
                case "V":
                    name = "co_cli";
                    table = "saFacturaVenta";

                    break;
                case "C":
                    name = "co_prov";
                    table = "saFacturaVenta";

                    break;
                case "PV":
                    name = "co_cli";
                    table = "saPlantillaVenta";

                    break;
                case "PC":
                    name = "co_prov";
                    table = "saPlantillaCompra";

                    break;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("SELECT doc_num, " + name + ", fec_emis, total_neto, status, impresa FROM " + table, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Invoice invoice = new Invoice()
                                {
                                    ID = reader["doc_num"].ToString(),
                                    Date = DateTime.Parse(reader["fec_emis"].ToString()),
                                    Amount = double.Parse(reader["total_neto"].ToString()),
                                    Status = int.Parse(reader["status"].ToString()),
                                    Printed = bool.Parse(reader["impresa"].ToString()),
                                    Type = type
                                };

                                string ID = "";
                                ProfitTMResponse getName = new ProfitTMResponse();

                                switch (type)
                                {
                                    case "V":
                                    case "PV":

                                        ID = reader["co_cli"].ToString();
                                        getName = new ClientManager().searchClient(ID);

                                        break;
                                    case "C":
                                    case "PC":

                                        ID = reader["co_prov"].ToString();
                                        getName = new SupplierManager().searchSupplier(ID);

                                        break;
                                }

                                if (getName.Status == "OK")
                                    invoice.Descrip = type == "V" || type == "PV" ? ((Client)getName.Result).Name : ((Supplier)getName.Result).Name;
                                else
                                    invoice.Descrip = ID;

                                invoices.Add(invoice);
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = invoices;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        public ProfitTMResponse getInvoiceItems(string id, string type)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            List<InvoiceItem> items = new List<InvoiceItem>();

            string proc = "";

            switch (type)
            {
                case "V":
                    proc = "pSeleccionarRenglonesFacturaVenta";

                    break;
                case "C":
                    proc = "pSeleccionarRenglonesFacturaCompra";

                    break;
                case "PV":
                    proc = "pSeleccionarRenglonesPlantillaVenta";

                    break;
                case "PC":
                    proc = "pSeleccionarRenglonesPlantillaCompra";

                    break;
            }

            string query = string.Format("exec {0} @sDoc_Num = '{1}'", proc, id);

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
                                items.Add(new InvoiceItem() {
                                    Reng = reader["reng_num"].ToString(),
                                    Code = reader["co_art"].ToString().Trim(),
                                    Name = reader["art_des"].ToString(),
                                    Quantity = double.Parse(reader["total_art"].ToString()),
                                    Amount = double.Parse(reader["reng_neto"].ToString()),
                                    IVA = double.Parse(reader["monto_imp"].ToString())
                                });
                            }
                        }
                    }
                }

                response.Status = "OK";
                response.Result = items;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }
            
            return response;
        }
        
        public ProfitTMResponse editInvoice(Invoice invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder query = new StringBuilder();

            ProfitTMResponse getDetails = getInvoiceDetails(invoice.Descrip, invoice.Type);

            if (getDetails.Status == "OK")
            {
                Dictionary<string, string> result = (Dictionary<string, string>)getDetails.Result;
                invoice.Cond = result["cond"].ToString();
                invoice.Seller = result["vend"].ToString();

                query.Append("UPDATE saFacturaVenta ");
                query.Append("SET co_cli = '" + invoice.Descrip + "', ");
                query.Append("co_cond = '" + invoice.Cond + "', ");
                query.Append("co_ven = '" + invoice.Seller + "' ");
                query.Append("WHERE doc_num = '" + invoice.ID + "'");

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
            }
            else
            {
                response.Status = "ERROR";
                response.Message = getDetails.Message;
            }

            return response;
        }
    }
}