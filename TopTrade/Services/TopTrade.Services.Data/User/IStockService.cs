namespace TopTrade.Services.Data.User
{
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;
    using TopTrade.Web.ViewModels.User;
    using TopTrade.Web.ViewModels.User.Profile;
    using TopTrade.Web.ViewModels.User.Stock;

    public interface IStockService
    {
        Task<StockSearchResultViewModel[]> SearchStocksBySymbolAsync(string stockNameOrTicker);

        Task<StockViewModel> GetStockDataAsync(string stockNameOrTicker);

        Task<StockTimeSeries> GetStockTimeSeriesAsync(string stockNameOrTicker);

        Task<UserDashboardViewModel> GetUserWatchlistWithStatisticAsync(string userId);

        StockBuyPercentTradesViewModel GetStockBuyPercentTrades(string ticker);

        Task RemoveStockFromWatchlistAsync(string input, string userId);

        Task UpdateUserStocksWatchlistAsync(StockSearchResultViewModel stock, string userId);

        Task<UserPortfolioVewModel> GetPortoflioAsync(string userId);

        Task UpdateUserStockWatchlistAsync(StockSearchResultViewModel stock, string userId);
    }
}
