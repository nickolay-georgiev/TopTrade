﻿namespace TopTrade.Services.Stock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using NewsAPI;
    using NewsAPI.Constants;
    using NewsAPI.Models;
    using TopTrade.Common;
    using TopTrade.Web.ViewModels.User;

    public class NewsService : INewsService
    {
        private readonly IConfiguration configuration;
        private readonly NewsApiClient newsApiClient;

        public NewsService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.newsApiClient = new NewsApiClient(this.configuration.GetSection(GlobalConstants.NewsApiKey).Value);
        }

        public async Task<IList<AllNewsViewModel>> GetLatestStocksNewsAsync()
        {
            ArticlesResult articlesResponse = await this.newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = "Stock market news",
                SortBy = SortBys.PublishedAt,
                Language = Languages.EN,
            });

            IList<AllNewsViewModel> articles = articlesResponse.Articles
                .GroupBy(x => x.Title)
                .Select(x => x.First())
                .Select(x => new AllNewsViewModel
                {
                    Title = x.Title ?? "Unknown",
                    Author = x.Author,
                    Description = x.Description,
                    Url = x.Url,
                    PublishedAt = x.PublishedAt,
                    ImageUrl = x.UrlToImage,
                }).Take(20).ToList();

            return articles;
        }

        public async Task<IList<AllNewsViewModel>> GetStockNewsByTickerAsync(string keyword)
        {

            ArticlesResult articlesResponse = await this.newsApiClient.GetEverythingAsync(new EverythingRequest
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
                    Title = x.Title ?? "Unknown",
                    Author = x.Author,
                    Description = x.Description,
                    Url = x.Url,
                    PublishedAt = x.PublishedAt,
                    ImageUrl = x.UrlToImage,
                }).Take(5).ToList();

            return articles;
        }
    }
}
