namespace TopTrade.Services
{
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;
    using TopTrade.Web.ViewModels.User;

    public interface IAlphaVantageApiClientService
    {
        Task<StockSearchResultViewModel[]> SearchStockBySymbolAsync(string stockNameOrTicker);

        Task<StockViewModel> GetStockDataAsync(string stockTicker);

        Task<StockTimeSeries> GetStockTimeSeriesAsync(string stockTicker);
    }
}
