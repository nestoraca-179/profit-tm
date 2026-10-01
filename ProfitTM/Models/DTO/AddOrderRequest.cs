using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models.DTO
{
    public class AddOrderRequest
    {
        public string ConnId { get; set; } // Connection ID for the database
        public string BranchId { get; set; } // Branch ID in selected database
        public string UserId { get; set; } // User ID making the request
        public saPedidoVenta Order { get; set; } // Order object for the request
    }
}