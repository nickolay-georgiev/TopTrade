namespace TopTrade.Services.Stock
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using NewsAPI;
    using NewsAPI.Constants;
    using NewsAPI.Models;
    using TopTrade.Common;
    using TopTrade.Web.ViewModels.User;

    public class NewsService : INewsService
    {
        private const string SearchFilter = "WALL STREET";
        private readonly IConfiguration configuration;
        private readonly NewsApiClient newsApiClient;

        public NewsService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.newsApiClient = new NewsApiClient(this.configuration.GetSection(GlobalConstants.NewsApiKey).Value);
        }

        public async Task<IList<AllNewsViewModel>> GetLatestStocksNewsAsync(string keyword)
        {
            keyword = keyword == "All" ? SearchFilter : keyword;

            ArticlesResult articlesResponse = await this.newsApiClient
                .GetEverythingAsync(new EverythingRequest
            {
                Q = keyword,
                SortBy = SortBys.PublishedAt,
                Language = Languages.EN,
            });

            IList<AllNewsViewModel> articles = articlesResponse.Articles
                .GroupBy(x => x.Title)
                .Select(x => x.First())
                .Select(x => new AllNewsViewModel
                {
                    Title = x.Title,
                    Author = x.Author ?? GlobalConstants.UnknownNewsAuthor,
                    Description = x.Description,
                    Url = x.Url,
                    PublishedAt = x.PublishedAt,
                    ImageUrl = x.UrlToImage ?? "/img/avatar-background-img.png",
                }).Take(20).ToList();

            return articles;
        }
    }
}
