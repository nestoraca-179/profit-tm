using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfitTM.Models.DTO
{
    public class CreditNoteDispatchRequest
    {
        public string ConnId { get; set; } // Connection ID for the database
        public string NroDoc { get; set; } // Document Number
    }
}