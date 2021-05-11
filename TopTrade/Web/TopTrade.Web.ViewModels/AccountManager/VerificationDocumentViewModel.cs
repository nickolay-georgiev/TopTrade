namespace TopTrade.Web.ViewModels.AccountManager
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class VerificationDocumentViewModel : IMapFrom<VerificationDocument>
    {
        [Display(Name ="User Id")]
        public string Id { get; set; }

        public string DocumentUrl { get; set; }

        public string VerificationStatus { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
