using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopTrade.Web.Areas.User
{
    public class HomeController : Controller
    {
        [Area("User")]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
