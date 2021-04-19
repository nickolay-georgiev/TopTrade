﻿namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.Controllers;

    public class StockSearchController : BaseApiController
    {
        [HttpPost]
        public string Search()
        {
            return "Search works";
        }
    }
}
