using ProfitTM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProfitTM.Controllers
{
    public class ProfitTMApiController : ApiController
    {
        // CONTROLADORES DEL API

        [HttpGet]
        [Route("api/ProfitTMApi/GetTreeReports/Prod/{prod}/Mod/{mod}")]
        public ProfitTMResponse GetReports(string prod, string mod)
        {
            ProfitTMResponse response;
            SQLController sqlController = new SQLController("MainConnection");

            response = sqlController.getReports(prod, mod);

            return response;
        }
    }
}