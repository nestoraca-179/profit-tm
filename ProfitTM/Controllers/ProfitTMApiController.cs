using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using ProfitTM.Models;

namespace ProfitTM.Controllers
{
    public class ProfitTMApiController : ApiController
    {
        // CADENA DE CONEXION
        // private readonly string connect = HttpContext.Current.Session["CONNECT"].ToString();

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
                response.Result = new_client.co_cli;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
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
                List<saPedidoVenta> orders = new Order().GetAllOrders(20, true);

                response.Status = "OK";
                response.Result = orders;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
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
            }

            return response;
        }

        // FACTURA

        [HttpPost]
        [Route("api/ProfitTMApi/AddInvoiceFromOrder/")]
        public ProfitTMResponse AddInvoiceFromOrder(saFacturaVenta invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            string user = (HttpContext.Current.Session["USER"] as Users).Username;
            string sucur = HttpContext.Current.Session["BRANCH"].ToString();

            try
            {
                saFacturaVenta new_invoice = new Invoice().AddFromOrder(invoice, user, sucur);

                if (new_invoice == null)
                {
                    response.Status = "ERROR";
                    response.Message = "Error agregando la factura";
                }
                else
                {
                    response.Status = "OK";
                    response.Result = new_invoice;
                }
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
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

                object stats = new Invoice().GetStatsInvoices(fecha_d, fecha_h);

                response.Status = "OK";
                response.Result = stats;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostSaleProducts/{fec_d}/{fec_h}/{number}")]
        public ProfitTMResponse GetMostSaleProducts(string fec_d, string fec_h, int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);

                List<saArticulo> arts = new Product().GetMostProducts(fecha_d, fecha_h, number, true);

                response.Status = "OK";
                response.Result = arts;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostPurchaseProducts/{fec_d}/{fec_h}/{number}")]
        public ProfitTMResponse GetMostPurchaseProducts(string fec_d, string fec_h, int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);

                List<saArticulo> arts = new Product().GetMostProducts(fecha_d, fecha_h, number, false);

                response.Status = "OK";
                response.Result = arts;
            }
            catch (Exception ex)
            {
                response.Status = "ERROR";
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostActiveClients/{fec_d}/{fec_h}/{number}")]
        public ProfitTMResponse GetMostActiveClients(string fec_d, string fec_h, int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);

                List<saCliente> clients = new Client().GetMostActiveClients(fecha_d, fecha_h, number);

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

        [HttpGet]
        [Route("api/ProfitTMApi/GetMostActiveSuppliers/{fec_d}/{fec_h}/{number}")]
        public ProfitTMResponse GetMostActiveSuppliers(string fec_d, string fec_h, int number)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            try
            {
                DateTime fecha_d = utils.FormatDate(fec_d);
                DateTime fecha_h = utils.FormatDate(fec_h);

                List<saProveedor> suppliers = new Supplier().GetMostActiveSuppliers(fecha_d, fecha_h, number);

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