using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ProfitTM.Controllers
{
    public class ProfitTMApiController : ApiController
    {
        // CONTROLADORES DEL API

        // Buscar reportes
        [HttpGet]
        [Route("api/ProfitTMApi/GetTreeReports/Prod/{prod}/Mod/{mod}")]
        public ProfitTMResponse GetReports(string prod, string mod)
        {
            ProfitTMResponse response;
            SQLController sqlController = new SQLController("MainConnection");

            response = sqlController.getReports(prod, mod);

            return response;
        }

        // Agregar proveedor
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

        // Modificar proveedor
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

        // Eliminar proveedor
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

        // Agregar cliente
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
    }
}