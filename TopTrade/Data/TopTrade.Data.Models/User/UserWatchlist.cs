namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using TopTrade.Data.Common.Models;

    public class UserWatchlist : BaseDeletableModel<int>
    {
        public UserWatchlist()
        {
            this.Stocks = new HashSet<UserWatchlistsStocks>();
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<UserWatchlistsStocks> Stocks { get; set; }
    }
}
