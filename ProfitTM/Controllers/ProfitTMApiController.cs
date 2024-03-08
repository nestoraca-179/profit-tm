using System;
using System.Web;
using System.Web.Http;
using System.Collections.Generic;
using ProfitTM.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ProfitTM.Controllers
{
    public class ProfitTMApiController : ApiController
    {
        // UTILS
        private readonly UtilsController utils = new UtilsController();

        // BUSCAR REPORTES
        [HttpGet]
        [Route("api/ProfitTMApi/GetReports/Prod/{prod}/Mod/{mod}")]
        public ProfitTMResponse GetReports(string prod, string mod)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                List<TreeReports> reports = Report.GetTreeReportsByProdMod(prod, mod);

                response.Status = "OK";
                response.Result = reports;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR BUSCANDO REPORTES", ex);
            }

            return response;
        }

        // USUARIO

        [HttpPost]
        [Route("api/ProfitTMApi/AddUser/")]
        public ProfitTMResponse AddUser(Users user)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                Users new_user = Models.User.Add(user);

                response.Status = "OK";
                response.Result = new_user.ID;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO USUARIO " + user.Username, ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditUser/")]
        public ProfitTMResponse EditUser(Users user)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                Users edit_user = Models.User.Edit(user);

                response.Status = "OK";
                response.Result = edit_user.ID;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR EDITANDO USUARIO " + user.Username, ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteUser/{id}/")]
        public ProfitTMResponse DeleteUser(int id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                Users del_user = Models.User.Delete(id);

                response.Status = "OK";
                response.Result = del_user.ID;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR ELIMINANDO USUARIO " + id.ToString(), ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/ResetPass/{id}")]
        public ProfitTMResponse ResetPass(int id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                Models.User.ResetPass(id);

                response.Status = "OK";
                response.Result = id;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR RESETANDO CLAVE DE USUARIO " + id.ToString(), ex);
            }

            return response;
        }

        // CAJA

        [HttpGet]
        [Route("api/ProfitTMApi/GetBoxes/{fec_d}")]
        public ProfitTMResponse GetBoxes(string fec_d)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                List<Box> boxes = Box.GetAllBoxesAndMoves(conn, fecha_d);

                response.Status = "OK";
                response.Result = boxes;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR BUSCANDO CAJAS", ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/AddBox/")]
        public ProfitTMResponse AddBox(Boxes box)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                Boxes new_box = Box.AddBox(box, user, sucur, conn);

                response.Status = "OK";
                response.Result = new_box;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO CAJA", ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/AddMove")]
        public ProfitTMResponse AddMove(saMovimientoCaja move)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                saMovimientoCaja new_move = new BoxMove().AddBoxMove(move, user, sucur, move.tipo_mov == "I", false, true, conn);

                response.Status = "OK";
                response.Result = new_move;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO MOVIMIENTO DE CAJA", ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/AddPayOrder")]
        public ProfitTMResponse AddPayOrder(saOrdenPago payOrder)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                saOrdenPago new_order = new PayOrder().AddPayOrder(payOrder, user, sucur, conn);

                response.Status = "OK";
                response.Result = new_order;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO ORDEN DE PAGO", ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/AddDocAdel")]
        public ProfitTMResponse AddDocAdel(saDocumentoCompra doc)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                saPago pay = new Pay().AddDocAdel(doc, user, sucur, conn);

                response.Status = "OK";
                response.Result = pay;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO ADELANTO A PROVEEDOR", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/CloseBox/{id}")]
        public ProfitTMResponse CloseBox(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                saMovimientoCaja move = Box.CloseBox(id, user, sucur, conn);

                response.Status = "OK";
                response.Result = move;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR CERRANDO CAJA " + id, ex);
            }

            return response;
        }

        // BANCO

        [HttpGet]
        [Route("api/ProfitTMApi/ConcilTransf/{id}/{cob_num}")]
        public ProfitTMResponse ConcilTransf(string id, string cob_num)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            string user = (HttpContext.Current.Session["USER"] as Users).Username;

            try
            {
                new Collect().ConcilCollect(id, cob_num, user);

                response.Status = "OK";
                response.Result = cob_num;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR CONCILIANDO TRANSFERENCIA", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/CancelTransf/{id}/{cob_num}")]
        public ProfitTMResponse CancelTransf(string id, string cob_num)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            string user = (HttpContext.Current.Session["USER"] as Users).Username;

            try
            {
                int result = new Collect().CancelCollect(id, cob_num, user);

                if (result == 0) 
                {
                    response.Status = "OK";
                    response.Result = result;
                }
                else
                {
                    response.Status = "ERROR";
                    switch (result)
                    {
                        case 1:
                            response.Message = "ADELANTO ASOCIADO A MAS DE UN CRUCE (COBRO)";
                            break;
                        case 2:
                            response.Message = "CRUCE CON OTROS TIPOS DE DOCUMENTOS";
                            break;
                        case 3:
                            response.Message = "ADELANTO CON MAS DE UNA TRANSFERENCIA";
                            break;
                        case 4:
                            response.Message = "COBRO CRUCE YA ANULADO";
                            break;
                        case 5:
                            response.Message = "COBRO ADELANTO YA ANULADO";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR ANULANDO TRANSFERENCIA", ex);
            }

            return response;
        }
        
        // CLIENTE

        [HttpPost]
        [Route("api/ProfitTMApi/AddClient/")]
        public ProfitTMResponse AddClient(saCliente client)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saCliente new_client = new Client().Add(client);

                response.Status = "OK";
                response.Result = new_client;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO CLIENTE " + client.cli_des, ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditClient/")]
        public ProfitTMResponse EditClient(saCliente client)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saCliente edit_client = new Client().Edit(client);

                response.Status = "OK";
                response.Result = edit_client.co_cli;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR EDITANDO CLIENTE " + client.cli_des, ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteClient/{id}/")]
        public ProfitTMResponse DeleteClient(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saCliente del_client = new Client().Delete(id);

                response.Status = "OK";
                response.Result = del_client.co_cli;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR ELIMINANDO CLIENTE " + id.ToString(), ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetPendingClientDocs/{client}/")]
        public ProfitTMResponse GetPendingClientDocs(string client)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                List<saDocumentoVenta> docs = new Client().GetPendingDocs(client);

                response.Status = "OK";
                response.Result = docs;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetClients/")]
        public ProfitTMResponse GetClients()
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                List<saCliente> clients = new Client().GetAllClients(false);

                response.Status = "OK";
                response.Result = clients;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR BUSCANDO CLIENTES", ex);
            }

            return response;
        }

        // PROVEEDOR

        [HttpPost]
        [Route("api/ProfitTMApi/AddSupplier/")]
        public ProfitTMResponse AddSupplier(saProveedor supplier)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saProveedor new_supplier = new Supplier().Add(supplier);

                response.Status = "OK";
                response.Result = new_supplier.co_prov;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO PROVEEDOR " + supplier.prov_des, ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditSupplier/")]
        public ProfitTMResponse EditSupplier(saProveedor supplier)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saProveedor edit_supplier = new Supplier().Edit(supplier);

                response.Status = "OK";
                response.Result = edit_supplier.co_prov;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR EDITANDO PROVEEDOR " + supplier.prov_des, ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteSupplier/{id}/")]
        public ProfitTMResponse DeleteSupplier(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saProveedor del_supplier = new Supplier().Delete(id);

                response.Status = "OK";
                response.Result = del_supplier.co_prov;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR ELIMINANDO PROVEEDOR " + id.ToString(), ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetPendingSupplierDocs/{supplier}")]
        public ProfitTMResponse GetPendingSupplierDocs(string supplier)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                List<saDocumentoVenta> docs = new Supplier().GetPendingDocs(supplier);

                response.Status = "OK";
                response.Result = docs;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        // PEDIDO VENTA

        [HttpGet]
        [Route("api/ProfitTMApi/GetOrder/{id}")]
        public ProfitTMResponse GetOrder(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saPedidoVenta order = new Order().GetOrderByID(id);

                if (order != null)
                {
                    if (order.status == "0")
                    {
                        response.Status = "OK";
                        response.Result = order;
                    }
                    else
                    {
                        response.Status = "ERROR";
                        response.Message = "1"; // PEDIDO BLOQUEADO
                    }
                }
                else
                {
                    response.Status = "ERROR";
                    response.Message = "0"; // PEDIDO NO EXISTE
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message; // HA OCURRIDO UN ERROR
                Incident.CreateIncident("ERROR BUSCANDO PEDIDO " + id, ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetOrders/{number}")]
        public ProfitTMResponse GetOrders(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                List<saPedidoVenta> orders = new Order().GetAllOrders(number, true);

                if (orders.Count > 0)
                {
                    response.Status = "OK";
                    response.Result = orders;
                }
                else
                {
                    response.Status = "ERROR";
                    response.Result = "No se encontraron preliquidaciones sin procesar";
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR BUSCANDO PRELIQUIDACIONES", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteOrder/{id}/")]
        public ProfitTMResponse DeleteOrder(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                saPedidoVenta del_order = new Order().Delete(id);

                response.Status = "OK";
                response.Result = del_order.doc_num;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR ELIMINANDO PRELIQUIDACION " + id, ex);
            }

            return response;
        }

        // FACTURA VENTA

        [HttpPost]
        [Route("api/ProfitTMApi/AddInvoice/{fromOrder}")]
        public ProfitTMResponse AddInvoice(int fromOrder, saFacturaVenta invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                saFacturaVenta new_invoice = new Invoice().AddSaleInvoice(invoice, user, sucur, conn, Convert.ToBoolean(fromOrder));

                response.Status = "OK";
                response.Result = new_invoice;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO FACTURA TRAS PRELIQUIDACION", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetInvoices/{number}")]
        public ProfitTMResponse GetInvoicesByNumber(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();

            try
            {
                List<saFacturaVenta> invoices = new Invoice().GetAllSaleInvoices(number, sucur);

                response.Status = "OK";
                response.Result = invoices;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR BUSCANDO FACTURAS", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetInvoice/{id}")]
        public ProfitTMResponse GetInvoice(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();

            try
            {
                saFacturaVenta invoice = new Invoice().GetSaleInvoiceByID(id);

                if (invoice != null)
                {
                    if (invoice.co_sucu_in.Trim() == sucur)
                    {
                        response.Status = "OK";
                        response.Result = invoice;
                    }
                    else
                    {
                        response.Status = "ERROR";
                        response.Message = "1"; // FACTURA DE OTRA SUCURSAL
                    }
                }
                else
                {
                    response.Status = "ERROR";
                    response.Message = "0"; // FACTURA NO EXISTE
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message; // HA OCURRIDO UN ERROR
                Incident.CreateIncident("ERROR BUSCANDO FACTURA " + id, ex);
            }

            return response;
        }
        
        [HttpGet]
        [Route("api/ProfitTMApi/SetPrintedInvoice/{id}")]
        public ProfitTMResponse SetPrintedInvoice(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                new Invoice().SetPrinted(id);

                response.Status = "OK";
                response.Result = id;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR MARCANDO FACTURA N° " + id + " COMO IMPRESA", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/CancelInvoice/{id}")]
        public async Task<ProfitTMResponse> CancelInvoiceAsync(string id)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string id_conn = HttpContext.Current.Session["ID_CONN"].ToString();
            string serie = new Branch().GetBranchByID(HttpContext.Current.Session["BRANCH"]?.ToString()).campo2;
            Connections conn = Connection.GetConnByID(id_conn);

            try
            {
                if (conn.Token == null || conn.DateToken == null || DateTime.Now > conn.DateToken)
                {
                    ModelAuthRequest auth = new ModelAuthRequest() { usuario = conn.UserToken, clave = conn.PassToken };
                    ModelAuthResponse res = await new Root().SendAuth(auth);

                    if (res.codigo == 200)
                    {
                        conn.Token = res.token;
                        conn.DateToken = res.expiracion.AddHours(-4);
                        Connection.Edit(conn);
                    }
                    else
                    {
                        throw new Exception(res.mensaje);
                    }
                }

                await new Invoice().SetCancelledAsync(id, user, serie, conn.Token);

                response.Status = "OK";
                response.Result = id;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR ANULANDO FACTURA N° " + id, ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/AddCreditNote/{doc_num}")]
        public ProfitTMResponse AddCreditNote(string doc_num)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();

            try
            {
                saDocumentoVenta new_doc = new Invoice().AddCreditNote(doc_num, user, sucur);

                response.Status = "OK";
                response.Result = new_doc;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO NOTA DE CREDITO", ex);
            }

            return response;
        }

        // FACTURA COMPRA

        [HttpGet]
        [Route("api/ProfitTMApi/GetBuyInvoices/{number}")]
        public ProfitTMResponse GetBuyInvoicesByNumber(int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            
            try
            {
                List<saFacturaCompra> invoices = new Invoice().GetAllBuyInvoices(number, sucur);

                response.Status = "OK";
                response.Result = invoices;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR BUSCANDO FACTURAS", ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/AddBuyInvoice/")]
        public ProfitTMResponse AddBuyInvoice(saFacturaCompra invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();

            try
            {
                saFacturaCompra new_invoice = new Invoice().AddBuyInvoice(invoice, user, sucur);

                response.Status = "OK";
                response.Result = new_invoice;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO FACTURA DE COMPRA", ex);
            }

            return response;
        }

        // COBRO

        [HttpPost]
        [Route("api/ProfitTMApi/AddCollect/")]
        public ProfitTMResponse AddCollect(saCobro cob)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();
            int conn = int.Parse(HttpContext.Current.Session["ID_CONN"].ToString());

            try
            {
                saCobro new_collect = new Collect().AddCollectFromInvoice(cob, user, sucur, conn);

                response.Status = "OK";
                response.Result = new_collect;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR AGREGANDO COBRO", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetCollectDocs/{co_cli}")]
        public ProfitTMResponse GetCollectDocs(string co_cli)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                List<saCobroDocReng> rengs = new Collect().GetCollectDocs(co_cli);

                response.Status = "OK";
                response.Result = rengs;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR BUSCANDO DOCUMENTOS EN MODULO DE COBROS", ex);
            }

            return response;
        }
        
        // ESTADISTICAS DASHBOARD ADMIN

        [HttpGet]
        [Route("api/ProfitTMApi/GetStatsInvoices/{fec_d}/{fec_h}")]
        public ProfitTMResponse GetStatsInvoices(string fec_d, string fec_h)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);
                string sucur = HttpContext.Current.Session["BRANCH"].ToString();

                object stats = new Invoice().GetStatsInvoices(fecha_d, fecha_h, sucur);

                response.Status = "OK";
                response.Result = stats;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR OBTENIENDO ESTADISTICAS DE VENTAS", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostSaleProducts/{fec_d}/{fec_h}/{number}/{suc}")]
        public ProfitTMResponse GetMostSaleProducts(string fec_d, string fec_h, int number, int suc)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);
                string sucur = HttpContext.Current.Session["BRANCH"].ToString();

                List<saArticulo> arts = new Product().GetMostProducts(fecha_d, fecha_h, number, true, suc == 1 ? sucur : null);

                response.Status = "OK";
                response.Result = arts;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR OBTENIENDO PRODUCTOS MAS VENDIDOS", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostPurchaseProducts/{fec_d}/{fec_h}/{number}/{suc}")]
        public ProfitTMResponse GetMostPurchaseProducts(string fec_d, string fec_h, int number, int suc)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);
                string sucur = HttpContext.Current.Session["BRANCH"].ToString();

                List<saArticulo> arts = new Product().GetMostProducts(fecha_d, fecha_h, number, false, suc == 1 ? sucur : null);

                response.Status = "OK";
                response.Result = arts;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR OBTENIENDO PRODUCTOS MAS COMPRADOS", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostActiveClients/{fec_d}/{fec_h}/{number}/{suc}")]
        public ProfitTMResponse GetMostActiveClients(string fec_d, string fec_h, int number, int suc)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);
                string sucur = HttpContext.Current.Session["BRANCH"].ToString();

                List<saCliente> clients = new Client().GetMostActiveClients(fecha_d, fecha_h, number, suc == 1 ? sucur : null);

                response.Status = "OK";
                response.Result = clients;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR OBTENIENDO CLIENTES MAS ACTIVOS", ex);
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostActiveSuppliers/{fec_d}/{fec_h}/{number}/{suc}")]
        public ProfitTMResponse GetMostActiveSuppliers(string fec_d, string fec_h, int number, int suc)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);
                string sucur = HttpContext.Current.Session["BRANCH"].ToString();

                List<saProveedor> suppliers = new Supplier().GetMostActiveSuppliers(fecha_d, fecha_h, number, suc == 1 ? sucur : null);

                response.Status = "OK";
                response.Result = suppliers;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR OBTENIENDO PROVEEDORES MAS ACTIVOS", ex);
            }

            return response;
        }
        
        // DECRIPTAR CLAVE

        [HttpGet]
        [Route("api/ProfitTMApi/DecryptPass/{username}")]
        public string DecryptPass(string username)
        {
            Users user = Models.User.GetUserByName(username);

            if (user != null)
                return SecurityController.Decrypt(user.Password);
            else
                return "USUARIO NO EXISTE";
        }

        // LOG

        [HttpPost]
        [Route("api/ProfitTMApi/EditLog/{fact}/")]
        public ProfitTMResponse EditLog(string fact, Root info)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                LogsFactOnline log = LogsFact.GetLogByID(fact);
                log.BodyJson = JsonConvert.SerializeObject(info);

                LogsFactOnline new_log = LogsFact.Edit(log);

                response.Status = "OK";
                response.Result = new_log.NroFact;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR EDITANDO LOG " + fact, ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/SendInvoice/")]
        public async Task<ProfitTMResponse> SendInvoiceAsync(ModelSendRequest request)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string id_conn = HttpContext.Current.Session["ID_CONN"].ToString();
            Connections conn = Connection.GetConnByID(id_conn);

            try
            {
                if (conn.Token == null || conn.DateToken == null || DateTime.Now > conn.DateToken)
                {
                    ModelAuthRequest auth = new ModelAuthRequest() { usuario = conn.UserToken, clave = conn.PassToken };
                    ModelAuthResponse r = await new Root().SendAuth(auth);

                    if (r.codigo == 200)
                    {
                        conn.Token = r.token;
                        conn.DateToken = r.expiracion.AddHours(-4);
                        Connection.Edit(conn);
                    }
                    else
                    {
                        throw new Exception(r.mensaje);
                    }
                }

                ModelSendResponse res = await new Root().SendEmail(request, conn.Token);

                response.Status = "OK";
                response.Result = res;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR ENVIANDO FACTURA " + request.numeroDocumento, ex);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/DownloadInvoice/")]
        public async Task<ProfitTMResponse> DownloadInvoiceAsync(ModelDownloadRequest request)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string id_conn = HttpContext.Current.Session["ID_CONN"].ToString();
            Connections conn = Connection.GetConnByID(id_conn);

            try
            {
                if (conn.Token == null || conn.DateToken == null || DateTime.Now > conn.DateToken)
                {
                    ModelAuthRequest auth = new ModelAuthRequest() { usuario = conn.UserToken, clave = conn.PassToken };
                    ModelAuthResponse r = await new Root().SendAuth(auth);

                    if (r.codigo == 200)
                    {
                        conn.Token = r.token;
                        conn.DateToken = r.expiracion.AddHours(-4);
                        Connection.Edit(conn);
                    }
                    else
                    {
                        throw new Exception(r.mensaje);
                    }
                }

                ModelDownloadResponse res = await new Root().DownloadInvoice(request, conn.Token);

                response.Status = "OK";
                response.Result = res;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
                Incident.CreateIncident("ERROR DESCARGANDO FACTURA " + request.numeroDocumento, ex);
            }

            return response;
        }
    }
}