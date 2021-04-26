namespace TopTrade.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Web.Controllers;

    public class TradeStockController : BaseApiController
    {
        [HttpPost]
        public string Sell()
        {
            return "test Sell Controller";
        }

        [HttpPost]
        public string Buy()
        {
            return "test Buy Controller";
        }
    }
}
