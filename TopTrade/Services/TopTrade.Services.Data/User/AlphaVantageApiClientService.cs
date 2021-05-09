namespace TopTrade.Services.Stock
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AlphaVantage.Net.Common.Intervals;
    using AlphaVantage.Net.Common.Size;
    using AlphaVantage.Net.Core.Client;
    using AlphaVantage.Net.Stocks;
    using AlphaVantage.Net.Stocks.Client;
    using Microsoft.Extensions.Configuration;
    using TopTrade.Common;
    using TopTrade.Web.ViewModels.User;

    public class AlphaVantageApiClientService : IAlphaVantageApiClientService
    {
        private readonly IConfiguration configuration;

        public AlphaVantageApiClientService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<StockSearchResultViewModel[]> SearchStockBySymbolAsync(string stockNameOrTicker)
        {
            using var client = new AlphaVantageClient(this.configuration.GetSection(GlobalConstants.AlphaVantageApiKey).Value);
            using var stocksClient = client.Stocks();
            var searchListResult = await stocksClient.SearchSymbolAsync(stockNameOrTicker);

            var topMatchResult = searchListResult
                .Where(x => x.Region == GlobalConstants.StockSearchRegion)
                .Take(5)
                .ToList();

            var stocks = topMatchResult.Select(x => new StockSearchResultViewModel
            {
                Name = x.Name,
                Ticker = x.Symbol,
            }).ToArray();

            return stocks;
        }

        public async Task<StockViewModel> GetStockDataAsync(string stockTicker)
        {
            using var client = new AlphaVantageClient(this.configuration.GetSection(GlobalConstants.AlphaVantageApiKey).Value);
            using var stocksClient = client.Stocks();

            var stockResult = await stocksClient.GetGlobalQuoteAsync(stockTicker);

            // var stockTimeSeries = await stocksClient

            // .GetTimeSeriesAsync(stockTicker, Interval.Daily, OutputSize.Compact, isAdjusted: true);

            // var dataset = stockTimeSeries.DataPoints.Select(x => x.ClosingPrice).ToList();
            var stock = new StockViewModel
            {
                Ticker = stockResult.Symbol,
                Price = stockResult.Price,
                Change = stockResult.Change,
                ChangePercent = stockResult.ChangePercent,
                DataSet = new List<decimal> { 1, 2, 3, 4, 5, 6 },
            };

            return stock;
        }

        public async Task<StockTimeSeries> GetStockTimeSeriesAsync(string stockTicker)
        {
            using var client = new AlphaVantageClient(this.configuration.GetSection(GlobalConstants.AlphaVantageApiKey).Value);
            using var stocksClient = client.Stocks();
            return await stocksClient.GetTimeSeriesAsync(stockTicker, Interval.Daily, OutputSize.Compact, isAdjusted: true);
        }
    }
}
