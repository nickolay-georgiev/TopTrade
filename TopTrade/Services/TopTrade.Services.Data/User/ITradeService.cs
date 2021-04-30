namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using TopTrade.Web.ViewModels.User.Stock;

    public interface ITradeService
    {
        IEnumerable<TradeHistoryViewModel> GetTradeHistory(string userId, int page, int itemsPerPage);
    }
}
