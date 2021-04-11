namespace TopTrade.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AlphaVantage.Net.Stocks;

    public interface IAlphaVantageApiClientService
    {
        Task<ICollection<SymbolSearchMatch>> SearchStock();

        Task<GlobalQuote> GetStockByTicker();

        Task<StockTimeSeries> GetStockByTicker1();
    }
}
