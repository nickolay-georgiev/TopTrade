namespace TopTrade.Web.Areas.User.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Services.Stock;

    public class NewsController : BaseLoggedUserController
    {
        private const string DefaultNewsSearch = "All";
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var newsViewModel = await this.newsService.GetLatestStocksNewsAsync(DefaultNewsSearch);
            return this.View(newsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            var newsViewModel = await this.newsService.GetLatestStocksNewsAsync(keyword);
            return this.View(nameof(this.Index), newsViewModel);
        }
    }
}
