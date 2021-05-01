namespace TopTrade.Web.ViewModels.User.Stock
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AutoMapper;
    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class TradeHistoryViewModel : PagingViewModel
    {
        public IEnumerable<TradeInHistoryViewModel> Trades { get; set; }
    }
}
