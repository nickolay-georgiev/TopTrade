namespace TopTrade.Services.Stock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache memoryCache;
        private readonly NewsApiClient newsApiClient;

        public NewsService(
            IConfiguration configuration,
            IMemoryCache memoryCache)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            this.newsApiClient = new NewsApiClient(this.configuration.GetSection(GlobalConstants.NewsApiKey).Value);
        }

        public async Task<IList<AllNewsViewModel>> GetLatestStocksNewsAsync(string keyword)
        {
            if (keyword == "All")
            {
                IList<AllNewsViewModel> newsViewModel;
                if (!this.memoryCache.TryGetValue<IList<AllNewsViewModel>>("newsCache", out newsViewModel))
                {
                    newsViewModel = await this.GetNews(SearchFilter);
                    this.memoryCache.Set("newsCache", newsViewModel, TimeSpan.FromMinutes(60));
                }

                return newsViewModel;
            }

            return await this.GetNews(keyword);
        }

        private async Task<IList<AllNewsViewModel>> GetNews(string keyword)
        {
            var articlesResponse = await this.newsApiClient
                            .GetEverythingAsync(new EverythingRequest
                            {
                                Q = keyword,
                                SortBy = SortBys.PublishedAt,
                                Language = Languages.EN,
                            });

            var articles = articlesResponse.Articles
                .GroupBy(x => x.Title)
                .Select(x => x.First())
                .Select(x => new AllNewsViewModel
                {
                    Title = x.Title,
                    Author = x.Author ?? GlobalConstants.UnknownNewsAuthor,
                    Description = x.Description,
                    Url = x.Url,
                    PublishedAt = x.PublishedAt,
                    ImageUrl = x.UrlToImage ?? GlobalConstants.DefaultNewsImage,
                }).Take(20).ToList();

            return articles;
        }
    }
}
