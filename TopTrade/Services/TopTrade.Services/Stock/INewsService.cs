namespace TopTrade.Services.Stock
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using NewsAPI.Models;
    using TopTrade.Web.ViewModels.User;

    public interface INewsService
    {
        Task<IList<AllNewsViewModel>> GetLatestStocksNewsAsync(string keyword);
    }
}
