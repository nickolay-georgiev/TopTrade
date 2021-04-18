using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTrade.Services.Data;
using TopTrade.Web.Controllers;

namespace TopTrade.Web.Areas.User
{
    public class HomeController : BaseController
    {
        private IAlphaVantageApiClientService stockService;

        public HomeController(IAlphaVantageApiClientService stockService)
        {
            this.stockService = stockService;
        }

        [Area("User")]
        public async Task<IActionResult> Index()
        {
            //var getStockByTicker = await this.stockService.GetStockByTicker();
            //var getStockByTicker1 = await this.stockService.GetStockByTicker1();
            //var searchStock = await this.stockService.SearchStock();

            return this.View();
        }


        public async Task<IActionResult> Test()
        {
            return this.View();
        }
    }
}
