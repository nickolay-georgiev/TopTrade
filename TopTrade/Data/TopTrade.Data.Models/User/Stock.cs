namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using TopTrade.Data.Common.Models;

    public class Stock : BaseDeletableModel<int>
    {
        public Stock()
        {
            this.Trades = new HashSet<Trade>();
            this.Watchlists = new HashSet<WatchlistStocks>();
        }

        [Required]
        [StringLength(40, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 1)]
        public string Ticker { get; set; }

        public virtual ICollection<Trade> Trades { get; set; }

        public virtual ICollection<WatchlistStocks> Watchlists { get; set; }
    }
}
