namespace TopTrade.Web.ViewModels.AccountManager
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class WithdrawRequestViewModel : IMapFrom<Withdraw>
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Display(Name = "Available")]
        public decimal UserAccountStatisticAvailable { get; set; }

        [Display(Name = "User Id")]
        public string UserId { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Status")]
        public string TransactionStatus { get; set; }
    }
}
