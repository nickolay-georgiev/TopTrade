namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using TopTrade.Data.Common.Models;

    public class Stock : BaseDeletableModel<int>
    {
        public Stock()
        {
            this.Trades = new HashSet<Trade>();
            this.Users = new HashSet<ApplicationUser>();
            this.Watchlists = new HashSet<WatchlistStocks>();
        }

        [Required]
        [StringLength(40, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 1)]
        public string Ticker { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        public double Change { get; set; }

        [Required]
        public double ChangePercent { get; set; }

        public virtual ICollection<Trade> Trades { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<WatchlistStocks> Watchlists { get; set; }
    }
}
