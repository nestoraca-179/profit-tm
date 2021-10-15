using ProfitTM.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
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

        public ProfitTMResponse addSaleInvoice(Invoice invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder queryF = new StringBuilder(), queryD = new StringBuilder(), queryI = new StringBuilder();

            invoice.InvoicePerson.Cond = Cond.GetCond(connect, invoice.InvoicePerson.Cond.ID);
            invoice.DateVenc = invoice.DateEmis.AddDays(invoice.InvoicePerson.Cond.DaysCred);

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();

                    queryF.AppendFormat("exec pInsertarFacturaVenta @sN_Control = '{0}', @sDoc_Num = '{1}', @sDescrip = NULL, ", invoice.ControlNumber, invoice.ID);
                    queryF.AppendFormat("@sCo_Cli = '{0}', @sCo_Cond = '{1}', @sCo_Cta_Ingre_Egr = NULL, ", invoice.InvoicePerson.ID, invoice.InvoicePerson.Cond.ID);
                    queryF.AppendFormat("@sCo_Tran = '{0}', @sCo_Ven = '{1}', @bContrib = '{2}', ", invoice.Transport.ID, invoice.InvoicePerson.Seller.ID, invoice.InvoicePerson.Contrib);
                    queryF.AppendFormat("@sCo_Mone = '{0}', @bAnulado = 0, @sdFec_emis = {1}, ", invoice.Currency.ID, invoice.DateEmis.ToString("MM/dd/yyyy HH:mm tt"));
                    queryF.AppendFormat("@sdFec_Venc = '{0}', @sdFec_Reg = '{1}', ", invoice.DateVenc.ToString("MM/dd/yyyy HH:mm tt"), DateTime.Now.ToString("MM/dd/yyyy HH:mm tt"));
                    queryF.AppendFormat("@sStatus = 0, @deTasa = {0}, @deMonto_Desc_Glob = 0, @deMonto_Reca = 0, @deSaldo = {1}, ", invoice.Rate.ToString().Replace(",", "."), invoice.Amount.ToString().Replace(",", "."));
                    queryF.AppendFormat("@deTotal_Bruto = {0}, @deMonto_Imp = {1}, ", invoice.SubTotal.ToString().Replace(",", "."), invoice.IVA.ToString().Replace(",", "."));
                    queryF.AppendFormat("@deMonto_Imp2 = 0, @deMonto_Imp3 = 0, @deOtros1 = 0, @deOtros2 = 0, @deOtros3 = 0, @deTotal_Neto = {0}, ", invoice.Amount.ToString().Replace(",", "."));
                    queryF.Append("@sComentario = NULL, @sDir_Ent = NULL, , @bImpresa = 0, @sSalestax = NULL, @sDis_Cen = NULL, ");
                    queryF.Append("@sCampo1 = NULL, @sCampo2 = NULL, @sCampo3 = NULL, @sCampo4 = NULL, @sCampo5 = NULL, @sCampo6 = NULL, @sCampo7 = NULL, @sCampo8 = NULL, @bVen_Ter = 0, @sImpfis = NULL, @sImpfisfac = NULL, ");
                    queryF.AppendFormat("@sco_us_in = '{0}', @sco_sucu_in = '{1}'", invoice.UserIn, invoice.BranchIn);

                    queryD.AppendFormat("exec pInsertarDocumentoVenta @sNro_Doc = '{0}', @sCo_Tipo_Doc = 'FACT', @sDoc_Orig = 'FACT', ", invoice.ID);
                    queryD.AppendFormat("@sCo_Cli = '{0}', @sCo_Mone = '{1}', @sNum_Comprobante = NULL, @deComis1 = 0, @deComis2 = 0, @deComis3 = 0, @deComis4 = 0, @deComis5 = 0, @deComis6 = 0, ", invoice.InvoicePerson.ID, invoice.Currency.ID);
                    queryD.AppendFormat("@sdFec_Emis = '{0}', @sdFec_Venc = '{1}', @sdFec_Reg = '{2}', ", invoice.DateEmis.ToString("MM/dd/yyyy HH:mm tt"), invoice.DateVenc.ToString("MM/dd/yyyy HH:mm tt"), DateTime.Now.ToString("MM/dd/yyyy HH:mm tt"));
                    queryD.AppendFormat("@bAnulado = 0, @deAdicional = 0, @sMov_Ban = NULL, @bAut = 1, @bContrib = {0}, ", invoice.InvoicePerson.Contrib);
                    queryD.AppendFormat("@sObserva = 'FACT N° {0} de Cliente {1}', @sNro_Orig = '{2}', @sNro_Che = NULL, ", invoice.ID, invoice.InvoicePerson.ID, invoice.ID);
                    queryD.AppendFormat("@sCo_Ven = '{0}', @sCo_Cta_Ingr_Egr = NULL, @deTasa = {1}, ", invoice.InvoicePerson.Seller.ID, invoice.Rate.ToString().Replace(",", "."));
                    queryD.AppendFormat("@sTipo_Imp = NULL, @deTotal_Bruto = {0}, @deTotal_Neto = {1}, @deMonto_Reca = 0, @deMonto_Imp = {2}, ", invoice.SubTotal.ToString().Replace(",", "."), invoice.Amount.ToString().Replace(",", "."), invoice.IVA.ToString().Replace(",", "."));
                    queryD.AppendFormat("@deMonto_Imp2 = 0, @deMonto_Imp3 = 0, @deSaldo = {0}, @sN_Control = '{1}', ", invoice.Amount.ToString().Replace(",", "."), invoice.ControlNumber);
                    queryD.Append("@deOtros1 = 0, @deOtros2 = 0, @deOtros3 = 0, @sPorc_Desc_Glob=NULL, @deMonto_Desc_Glob = 0, @sPorc_Reca = NULL, @dePorc_Imp = 0, @dePorc_Imp2 = 0, @dePorc_Imp3 = 0, @sSalestax = NULL, @bVen_Ter = 0, ");
                    queryD.Append("@sCampo1 = NULL, @sCampo2 = NULL, @sCampo3 = NULL, @sCampo4 = NULL, @sCampo5 = NULL, @sCampo6 = NULL, @sCampo7 = NULL, @sCampo8 = NULL, ");
                    queryD.AppendFormat("@sco_us_in = '{0}', @sco_sucu_in = '{1}'", invoice.UserIn, invoice.BranchIn);

                    using (SqlCommand comm = new SqlCommand(queryF.ToString(), conn))
                    {
                        int rows = comm.ExecuteNonQuery();

                        foreach (InvoiceItem item in invoice.Items)
                        {
                            if (item.TypeArt == "V")
                            {
                                string queryDES = string.Format("exec pStockActualizar @sCo_Alma='{0}',@sCo_Art='{1}',@sCo_Uni='{2}',@deCantidad={3},@sTipoStock='DES',@bSumarStock=1,@bPermiteStockNegativo=0",
                                    item.Storage.ID,
                                    item.Code,
                                    item.Unit,
                                    item.Quantity
                                );

                                string queryACT = string.Format("exec pStockActualizar @sCo_Alma='{0}',@sCo_Art='{1}',@sCo_Uni='{2}',@deCantidad={3},@sTipoStock='ACT',@bSumarStock=0,@bPermiteStockNegativo=0",
                                    item.Storage.ID,
                                    item.Code,
                                    item.Unit,
                                    item.Quantity
                                );

                                comm.CommandText = queryDES;
                                rows = comm.ExecuteNonQuery();

                                comm.CommandText = queryACT;
                                rows = comm.ExecuteNonQuery();
                            }

                            queryI.AppendFormat("exec pInsertarRenglonesFacturaVenta @sDoc_Num = '{0}', @sCo_Art = '{1}', ", invoice.ID, item.Code);
                            queryI.AppendFormat("@sDes_Art = '{0}', @sCo_Uni = '{1}', @sCo_Alma = '{2}', @sCo_Precio = '{3}', ", item.Name, item.Unit, item.Storage.ID, item.PriceCode);
                            queryI.AppendFormat("@sTipo_imp = '{0}', @deTotal_Art = {1}, @deStotal_Art = 0, ", item.ImpCode, item.Quantity.ToString().Replace(",", "."));
                            queryI.AppendFormat("@dePrec_Vta = {0}, @dePorc_Imp = {1}, @dePorc_Imp2 = 0, @dePorc_Imp3 = 0, ", (item.Amount / item.Quantity).ToString().Replace(",", "."), item.ImpPorc.Replace(",", "."));
                            queryI.AppendFormat("@deReng_Neto = {0}, @dePendiente = {1}, @dePendiente2 = 0, @sTipoDoc = NULL, ", item.Amount.ToString().Replace(",", "."), item.Quantity.ToString());
                            queryI.AppendFormat("@gRowguid_Doc = NULL, @deMonto_Imp = {0}, @deTotal_Dev = 0, @deMonto_Dev = 0, @deOtros = 0, @deMonto_Imp2 = 0, @deMonto_Imp3 = 0, ", item.IVA.ToString().Replace(",", "."));
                            queryI.Append("@sComentario = NULL, @deMonto_Desc_Glob = 0, @deMonto_Reca_Glob = 0, @deOtros1_Glob = 0, @deOtros2_glob = 0, @deOtros3_glob = 0, ");
                            queryI.Append("@deMonto_imp_afec_glob = 0, @deMonto_imp2_afec_glob = 0, @deMonto_imp3_afec_glob = 0,@iRENG_NUM = 1, @sREVISADO = NULL, @sTRASNFE = NULL, ");
                            queryI.AppendFormat("@sco_us_in = '{0}', @sco_sucu_in = '{1}'", invoice.UserIn, invoice.BranchIn);

                            comm.CommandText = queryI.ToString();
                            rows = comm.ExecuteNonQuery();
                            queryI.Clear();
                        }

                        comm.CommandText = queryD.ToString();
                        rows = comm.ExecuteNonQuery();
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

        public ProfitTMResponse editInvoice(Invoice invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            StringBuilder queryF = new StringBuilder(), queryD = new StringBuilder();

            string name = "", tableF = "", tableD = "";
            bool invoiceSale = false;

            switch (invoice.Type)
            {
                case "V":
                    invoiceSale = true;
                    name = "co_cli";
                    tableF = "saFacturaVenta";
                    tableD = "saDocumentoVenta";

                    break;
                case "C":
                    name = "co_prov";
                    tableF = "saFacturaCompra";
                    tableD = "saDocumentoCompra";

                    break;
                case "PV":
                    invoiceSale = true;
                    name = "co_cli";
                    tableF = "saPlantillaVenta";

                    break;
                case "PC":
                    name = "co_prov";
                    tableF = "saPlantillaCompra";

                    break;
            }

            invoice.InvoicePerson.Cond = Cond.GetCond(connect, invoice.InvoicePerson.Cond.ID);
            invoice.DateVenc = invoice.DateEmis.AddDays(invoice.InvoicePerson.Cond.DaysCred);

            queryF.Append(string.Format("UPDATE {0} ", tableF));
            queryF.Append(string.Format("SET {0} = '{1}', ", name, invoice.InvoicePerson.ID));

            if (invoiceSale)
            {
                queryF.Append(string.Format("co_cond = '{0}', ", invoice.InvoicePerson.Cond.ID));
                queryF.Append(string.Format("co_ven = '{0}', ", invoice.InvoicePerson.Seller.ID));
                queryF.Append(string.Format("co_tran = '{0}', ", invoice.Transport.ID));
            }

            queryF.Append(string.Format("tasa = {0}, ", invoice.Rate.ToString().Replace(",", ".")));
            queryF.Append(string.Format("n_control = '{0}', ", invoice.ControlNumber));
            queryF.Append(string.Format("fec_venc = '{0}' ", invoice.DateVenc.ToString("yyyy/MM/dd HH:mm")));
            queryF.Append(string.Format("WHERE doc_num = '" + invoice.ID + "'"));

            if (tableD != "")
            {
                queryD.Append(string.Format("UPDATE {0} ", tableD));
                queryD.Append(string.Format("SET {0} = '{1}', ", name, invoice.InvoicePerson.ID));

                if (invoiceSale)
                {
                    queryD.Append(string.Format("co_ven = '{0}', ", invoice.InvoicePerson.Seller.ID));
                    queryD.Append(string.Format("n_control = '{0}', ", invoice.ControlNumber));
                }

                queryD.Append(string.Format("tasa = {0}, ", invoice.Rate.ToString().Replace(",", ".")));
                queryD.Append(string.Format("fec_venc = '{0}' ", invoice.DateVenc.ToString("yyyy/MM/dd HH:mm")));
                queryD.Append(string.Format("WHERE co_tipo_doc = 'FACT' AND nro_doc = '" + invoice.ID + "'"));
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(queryF.ToString(), conn))
                    {
                        int rowsF = comm.ExecuteNonQuery();
                        comm.CommandText = queryD.ToString();
                        int rowsD = comm.ExecuteNonQuery();

                        if (rowsF > 0 && rowsD > 0)
                        {
                            response.Status = "OK";
                            response.Result = rowsF + rowsD;
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