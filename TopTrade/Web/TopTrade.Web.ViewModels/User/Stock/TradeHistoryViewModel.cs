namespace TopTrade.Web.ViewModels.User.Stock
{
    using System.Collections.Generic;

    public class TradeHistoryViewModel : PagingViewModel
    {
        public IEnumerable<TradeInHistoryViewModel> Trades { get; set; }

        public string ProfitLossBarChartViewModel { get; set; }

        public string MonthlyWalletPerformanceChartViewModel { get; set; }
    }
}
