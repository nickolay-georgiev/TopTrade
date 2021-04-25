namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using TopTrade.Data.Common.Models;

    public class UserWatchlistsStocks
    {
        public int UserWatchlistId { get; set; }

        public virtual UserWatchlist UserWatchlist { get; set; }

        public int StockId { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
