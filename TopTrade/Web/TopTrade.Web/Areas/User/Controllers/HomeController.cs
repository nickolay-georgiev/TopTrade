namespace TopTrade.Web.Areas.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data;
    using TopTrade.Web.Areas.User.Controllers;
    using TopTrade.Web.Controllers;

    public class HomeController : BaseLoggedUserController
    {
        private readonly IAlphaVantageApiClientService stockService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(
            IAlphaVantageApiClientService stockService,
            SignInManager<ApplicationUser> signInManager)
        {
            this.stockService = stockService;
            this.signInManager = signInManager;
        }

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
