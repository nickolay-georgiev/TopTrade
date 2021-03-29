namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    
    public class TradePlatformController : Controller
    {
        [Area("User")]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
