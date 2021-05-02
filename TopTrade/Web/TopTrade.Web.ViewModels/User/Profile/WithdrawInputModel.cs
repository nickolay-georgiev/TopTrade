namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class WithdrawInputModel : IValidatableObject
    {
        public decimal Amount { get; set; }

        public decimal Available { get; set; }

        public string Card { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Amount > this.Available)
            {
                yield return new ValidationResult($"You can withdraw up to {this.Available}");
            }
        }
    }
}
