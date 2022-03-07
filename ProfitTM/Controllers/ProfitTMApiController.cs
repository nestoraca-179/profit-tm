using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace ProfitTM.Controllers
{
    public class ProfitTMApiController : ApiController
    {
        // CADENA DE CONEXION
        private readonly string connect = HttpContext.Current.Session["connect"].ToString();

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

        // FACTURA

        /*[HttpPost]
        [Route("api/ProfitTMApi/AddInvoice/{order}/")]
        public ProfitTMResponse AddInvoice(string order, Invoice invoice)
        {
            ProfitTMResponse response = new ProfitTMResponse();

            InvoiceManager invoiceManager = new InvoiceManager(connect);

            if (invoice.Type == "V")
            {
                response = invoiceManager.addSaleInvoice(invoice, order);
            }

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditInvoice/")]
        public ProfitTMResponse EditInvoice(Invoice invoice)
        {
            ProfitTMResponse response;

            InvoiceManager invoiceManager = new InvoiceManager(connect);
            response = invoiceManager.editInvoice(invoice);

            return response;
        }*/
    }
}