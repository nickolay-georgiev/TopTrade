namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class WithdrawInputModel : IValidatableObject
    {
        [Required]
        [Range(typeof(decimal), "1", "99999999999999")]
        public decimal DesiredAmount { get; set; }

        [Required]
        public decimal Available { get; set; }

        [Required]
        public string Card { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.DesiredAmount > this.Available)
            {
                yield return new ValidationResult($"You can withdraw up to {this.Available}");
            }
        }
    }
}
