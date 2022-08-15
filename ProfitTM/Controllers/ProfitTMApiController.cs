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
        // private readonly string connect = HttpContext.Current.Session["connect"].ToString();

        // UTILS
        private readonly UtilsController utils = new UtilsController();

        // CONTROLADORES DEL API

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
                Users newUser = Models.User.Add(user);

                response.Status = "OK";
                response.Result = newUser.ID;
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
                Users editUser = Models.User.Edit(user);

                response.Status = "OK";
                response.Result = editUser.ID;
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
                Users delUser = Models.User.Delete(id);

                response.Status = "OK";
                response.Result = delUser.ID;
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
                saCliente newClient = new Client().Add(client);

                response.Status = "OK";
                response.Result = newClient.co_cli;
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
                saCliente editClient = new Client().Edit(client);

                response.Status = "OK";
                response.Result = editClient.co_cli;
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
                saCliente delClient = new Client().Delete(id);

                response.Status = "OK";
                response.Result = delClient.co_cli;
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
                saProveedor newSupplier = new Supplier().Add(supplier);

                response.Status = "OK";
                response.Result = newSupplier.co_prov;
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
                saProveedor editSupplier = new Supplier().Edit(supplier);

                response.Status = "OK";
                response.Result = editSupplier.co_prov;
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
                saProveedor delSupplier = new Supplier().Delete(id);

                response.Status = "OK";
                response.Result = delSupplier.co_prov;
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