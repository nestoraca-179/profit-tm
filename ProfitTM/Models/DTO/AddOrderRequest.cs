using Newtonsoft.Json;
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

        public void LogRequest()
        {
            try
            {
                string data = $"ConnId: {ConnId}, BranchId: {BranchId}, UserId: {UserId}";
                string request = JsonConvert.SerializeObject(new { Data = data, saPedidoVenta = Order }, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                LogsFact.CreateLogInFile($"ORDER_ADD_REQUEST {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {request}");
            }
            catch (Exception ex)
            {
                LogsFact.CreateLogInFile($"ORDER_ADD_LOG_ERROR {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.Message}");
            }
        }
    }
}