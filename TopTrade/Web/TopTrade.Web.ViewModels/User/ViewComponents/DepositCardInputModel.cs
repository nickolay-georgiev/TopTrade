namespace TopTrade.Web.ViewModels.User.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class DepositCardInputModel
    {
        [Required]
        [RegularExpression(
            "^[0-9]{4}-[0-9]{4}-[0-9]{4}-[0-9]{4}$",
            ErrorMessage = "Card number can contains only digits and must be exact 16 digits long")]
        public string Number { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "First name can not be more that 20 characters long")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(20, MinimumLength = 0, ErrorMessage = "Middle name can not be more that 20 characters long")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Last name can not be more that 20 characters long")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
