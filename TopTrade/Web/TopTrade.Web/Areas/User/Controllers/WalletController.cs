namespace TopTrade.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.Controllers;

    public class WalletController : BaseLoggedUserController
    {
        [Area("User")]
        public async Task<IActionResult> Index()
        {
            // var getStockByTicker = await this.stockService.GetStockByTicker();
            // var getStockByTicker1 = await this.stockService.GetStockByTicker1();
            // var searchStock = await this.stockService.SearchStock();

            return this.View();
        }
    }
}
