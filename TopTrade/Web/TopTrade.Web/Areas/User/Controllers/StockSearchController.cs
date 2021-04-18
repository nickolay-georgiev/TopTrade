using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTrade.Web.Controllers;

namespace TopTrade.Web.Areas.User.Controllers
{
    public class StockSearchController : BaseApiController
    {
        [HttpPost]
        public string Search()
        {
            return "Search works";
        }
    }
}
