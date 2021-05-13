namespace TopTrade.Web.ViewModels.Administration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using TopTrade.Data.Models;
    using TopTrade.Services.Mapping;

    public class AccountManagerViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedOn { get; set; }

        [Display(Name = "Is Deleted")]
        public string IsDeleted { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => this.FirstName == null && this.LastName == null
            ? "none" : $"{this.FirstName} {this.LastName}";
    }
}
