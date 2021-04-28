namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using TopTrade.Data.Common.Models;

    public class WatchlistStocks : BaseDeletableModel<int>
    {
        public int WatchlistId { get; set; }

        public virtual Watchlist Watchlist { get; set; }

        public int StockId { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
