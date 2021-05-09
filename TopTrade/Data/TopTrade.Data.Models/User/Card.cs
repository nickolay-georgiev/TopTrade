namespace TopTrade.Data.Models.User
{
    using System.ComponentModel.DataAnnotations;

    using TopTrade.Data.Common.Models;

    public class Card : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(19, MinimumLength = 19)]
        public string Number { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
