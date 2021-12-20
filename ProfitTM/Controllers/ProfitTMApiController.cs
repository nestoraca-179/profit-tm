using ProfitTM.Models;
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
            ProfitTMResponse response;
            SQLController sqlController = new SQLController("MainConnection");

            response = sqlController.getReports(prod, mod);

            return response;
        }

        // USUARIO
        
        [HttpPost]
        [Route("api/ProfitTMApi/AddUser/")]
        public ProfitTMResponse AddUser(User user)
        {
            ProfitTMResponse response;

            UserManager userManager = new UserManager();
            response = userManager.addUser(user);

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditUser/")]
        public ProfitTMResponse EditUser(User user)
        {
            ProfitTMResponse response;

            UserManager userManager = new UserManager();
            response = userManager.editUser(user);

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteUser/{id}/")]
        public ProfitTMResponse DeleteUser(int id)
        {
            ProfitTMResponse response;

            UserManager userManager = new UserManager();
            response = userManager.deleteUser(id);

            return response;
        }

        // CLIENTE

        [HttpPost]
        [Route("api/ProfitTMApi/AddClient/")]
        public ProfitTMResponse AddClient(Client client)
        {
            ProfitTMResponse response;

            ClientManager clientManager = new ClientManager(connect);
            response = clientManager.addClient(client);

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditClient/")]
        public ProfitTMResponse EditClient(Client client)
        {
            ProfitTMResponse response;

            ClientManager clientManager = new ClientManager(connect);
            response = clientManager.editClient(client);

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteClient/{id}/")]
        public ProfitTMResponse DeleteClient(string id)
        {
            ProfitTMResponse response;

            ClientManager clientManager = new ClientManager(connect);
            response = clientManager.deleteClient(id);

            return response;
        }

        // PROVEEDOR

        [HttpPost]
        [Route("api/ProfitTMApi/AddSupplier/")]
        public ProfitTMResponse AddSupplier(Supplier supplier)
        {
            ProfitTMResponse response;

            SupplierManager supplierManager = new SupplierManager(connect);
            response = supplierManager.addSupplier(supplier);

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditSupplier/")]
        public ProfitTMResponse EditSupplier(Supplier supplier)
        {
            ProfitTMResponse response;

            SupplierManager supplierManager = new SupplierManager(connect);
            response = supplierManager.editSupplier(supplier);

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteSupplier/{id}/")]
        public ProfitTMResponse DeleteSupplier(string id)
        {
            ProfitTMResponse response;

            SupplierManager supplierManager = new SupplierManager(connect);
            response = supplierManager.deleteSupplier(id);

            return response;
        }

        // FACTURA

        [HttpPost]
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
        }
    }
}