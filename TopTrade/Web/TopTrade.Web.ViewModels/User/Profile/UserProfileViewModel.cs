namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.ComponentModel.DataAnnotations;

    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Address { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 2)]
        public string ZipCode { get; set; }

        [Display(Name = "About Me")]
        public string AboutMe { get; set; }

        public string AvatarUrl { get; set; }

        public UserAvatarInputModel UserAvatarInputModel { get; set; }
    }
}
