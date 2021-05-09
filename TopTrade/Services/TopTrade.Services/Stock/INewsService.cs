namespace TopTrade.Services.Stock
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TopTrade.Web.ViewModels.User;

    public interface INewsService
    {
        Task<IList<AllNewsViewModel>> GetLatestStocksNewsAsync(string keyword);
    }
}
