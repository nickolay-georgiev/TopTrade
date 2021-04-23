namespace TopTrade.Web.ViewModels.User.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class DepositCardInputModel
    {

        [Required(ErrorMessage = "Card number can contains only digits")]
        [RegularExpression("^[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}$")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Card expiration date can contains only digits")]
        [RegularExpression("^[0-1][0-9] / [2-9][0-9]$")]
        public string ExpirationDate { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
