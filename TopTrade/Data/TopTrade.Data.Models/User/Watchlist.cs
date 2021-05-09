namespace TopTrade.Data.Models.User
{
    using System.Collections.Generic;

    using TopTrade.Data.Common.Models;

    public class Watchlist : BaseDeletableModel<int>
    {
        public Watchlist()
        {
            this.Stocks = new HashSet<WatchlistStocks>();
        }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<WatchlistStocks> Stocks { get; set; }
    }
}
