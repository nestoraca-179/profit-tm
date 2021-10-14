using ProfitTM.Models;
using System.Web;
using System.Web.Http;

namespace ProfitTM.Controllers
{
    public class ProfitTMApiController : ApiController
    {
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

        // BUSCAR OPCIONES
        [HttpGet]
        [Route("api/ProfitTMApi/GetOptions/Mod/{mod}/Type/{type}")]
        public ProfitTMResponse GetOptions(string mod, char type)
        {
            ProfitTMResponse response;
            SQLController sqlController = new SQLController("MainConnection");

            response = sqlController.getOptions(mod, type);

            return response;
        }

        // PROVEEDOR

        [HttpPost]
        [Route("api/ProfitTMApi/AddSupplier/")]
        public ProfitTMResponse AddSupplier(Supplier supplier)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            SupplierManager supplierManager = new SupplierManager(connect);
            response = supplierManager.addSupplier(supplier);

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditSupplier/")]
        public ProfitTMResponse EditSupplier(Supplier supplier)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            SupplierManager supplierManager = new SupplierManager(connect);
            response = supplierManager.editSupplier(supplier);

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteSupplier/{id}/")]
        public ProfitTMResponse DeleteSupplier(string id)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            SupplierManager supplierManager = new SupplierManager(connect);
            response = supplierManager.deleteSupplier(id);

            return response;
        }

        // CLIENTE

        [HttpPost]
        [Route("api/ProfitTMApi/AddClient/")]
        public ProfitTMResponse AddClient(Client client)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            ClientManager clientManager = new ClientManager(connect);
            response = clientManager.addClient(client);

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditClient/")]
        public ProfitTMResponse EditClient(Client client)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            ClientManager clientManager = new ClientManager(connect);
            response = clientManager.editClient(client);

            return response;
        }

        [HttpGet]
        [Route("api/ProfitTMApi/DeleteClient/{id}/")]
        public ProfitTMResponse DeleteClient(string id)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            ClientManager clientManager = new ClientManager(connect);
            response = clientManager.deleteClient(id);

            return response;
        }

        // FACTURA

        [HttpPost]
        [Route("api/ProfitTMApi/AddInvoice/")]
        public ProfitTMResponse AddInvoice(Invoice invoice)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            InvoiceManager invoiceManager = new InvoiceManager(connect);
            response = invoiceManager.addInvoice(invoice);

            return response;
        }

        [HttpPost]
        [Route("api/ProfitTMApi/EditInvoice/")]
        public ProfitTMResponse EditInvoice(Invoice invoice)
        {
            ProfitTMResponse response;
            string connect = HttpContext.Current.Session["connect"].ToString();

            InvoiceManager invoiceManager = new InvoiceManager(connect);
            response = invoiceManager.editInvoice(invoice);

            return response;
        }
    }
}