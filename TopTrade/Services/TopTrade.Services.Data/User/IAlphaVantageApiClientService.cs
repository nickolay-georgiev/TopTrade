namespace TopTrade.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;
    using TopTrade.Web.ViewModels.User;

    public interface IAlphaVantageApiClientService
    {
        Task<StockSearchResultViewModel[]> SearchStockBySymbol(string stockNameOrTicker);

        Task<StockViewModel> GetStockByTicker(string stockTicker);

        Task<StockTimeSeries> GetStockTimeSeries(string stockTicker);
    }
}
