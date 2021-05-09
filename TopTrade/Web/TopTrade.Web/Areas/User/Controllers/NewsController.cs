namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Stock;

    public class NewsController : BaseLoggedUserController
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var news = await this.newsService.GetLatestStocksNewsAsync("All");
            return this.View(news);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            var news = await this.newsService.GetLatestStocksNewsAsync(keyword);
            return this.View(nameof(this.Index), news);
        }
    }
}
