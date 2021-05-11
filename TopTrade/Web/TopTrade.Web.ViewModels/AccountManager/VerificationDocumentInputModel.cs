namespace TopTrade.Web.ViewModels.AccountManager
{
    using System.ComponentModel.DataAnnotations;

    using TopTrade.Data.Models.User.Enums;

    public class VerificationDocumentInputModel
    {
        public string Id { get; set; }

        [Required]
        [EnumDataType(typeof(VerificationDocumentStatus))]
        public string VerificationStatus { get; set; }
    }
}
