namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class TradeSharesController : Controller
    {
        [HttpGet]
        [IgnoreAntiforgeryToken]
        public string Post()
        {
            return "test Api Controller";
        }
    }
}
