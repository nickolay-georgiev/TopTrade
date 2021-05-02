namespace TopTrade.Web.ViewModels.User.Profile
{
    using AutoMapper;
    using System;

    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class WithdrawInWalletHistoryViewModel : IMapFrom<Withdraw>
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CardNumber { get; set; }

        public string TransactionStatus { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Withdraw, WithdrawInWalletHistoryViewModel>()
        //       .ForMember(x => x.card, opt =>
        //           opt.MapFrom(x => x.Stock.Name));
        //}
    }
}
