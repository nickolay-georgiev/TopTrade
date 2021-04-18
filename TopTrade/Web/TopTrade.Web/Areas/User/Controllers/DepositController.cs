namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.Controllers;

    public class DepositController : BaseApiController
    {
        [HttpGet]
        public string Test()
        {
            return "Deposit Get Wokrs";
        }

        [HttpPost]
        public object Test1(object test)
        {
            return "Deposit Post Wokrs";
        }
    }
}
