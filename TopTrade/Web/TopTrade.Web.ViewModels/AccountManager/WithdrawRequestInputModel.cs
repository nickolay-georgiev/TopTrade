namespace TopTrade.Web.ViewModels.AccountManager
{
    using System.ComponentModel.DataAnnotations;

    using TopTrade.Data.Models.User.Enums;

    public class WithdrawRequestInputModel
    {
        public int Id { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionStatus))]
        public string TransactionStatus { get; set; }
    }
}
