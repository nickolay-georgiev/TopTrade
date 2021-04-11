namespace TopTrade.Services.Data
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;

    using AlphaVantage.Net.Common;
    using AlphaVantage.Net.Common.Intervals;
    using AlphaVantage.Net.Common.Size;
    using AlphaVantage.Net.Core.Client;
    using AlphaVantage.Net.Stocks;
    using AlphaVantage.Net.Stocks.Client;

    public class AlphaVantageApiClientService : IAlphaVantageApiClientService
    {

        public async Task<ICollection<SymbolSearchMatch>> SearchStock()
        {
            using var client = new AlphaVantageClient("AYEDU2WZ2YC86XC0");
            using var stocksClient = client.Stocks();
            return await stocksClient.SearchSymbolAsync("BA");
        }

        public async Task<GlobalQuote> GetStockByTicker()
        {
            using var client = new AlphaVantageClient("AYEDU2WZ2YC86XC0");
            using var stocksClient = client.Stocks();
            return await stocksClient.GetGlobalQuoteAsync("AAPL");
        }

        public async Task<StockTimeSeries> GetStockByTicker1()
        {
            using var client = new AlphaVantageClient("AYEDU2WZ2YC86XC0");
            using var stocksClient = client.Stocks();
            return await stocksClient.GetTimeSeriesAsync("AAPL", Interval.Daily, OutputSize.Compact, isAdjusted: true);
        }
    }
}
