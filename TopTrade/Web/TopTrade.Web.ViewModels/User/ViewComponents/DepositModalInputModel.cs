namespace TopTrade.Web.ViewModels.User.ViewComponents
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DepositModalInputModel
    {
        [Required(ErrorMessage = "Amount must be between $10 and $50.000")]
        [Range(10, 50_000)]
        public decimal Amount { get; set; }

        public bool IsVerified { get; set; }

        public DepositCardInputModel Card { get; set; }
    }
}
