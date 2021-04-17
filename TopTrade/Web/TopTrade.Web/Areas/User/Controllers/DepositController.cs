namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class DepositController : BaseController
    {
        [HttpGet]
       public string Test()
        {
            return "Deposit Wokrs";
        }
    }
}
