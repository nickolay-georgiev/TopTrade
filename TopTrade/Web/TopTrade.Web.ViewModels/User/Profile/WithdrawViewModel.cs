namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using TopTrade.Data.Models.User;
    using TopTrade.Services.Mapping;

    public class WithdrawViewModel : IMapFrom<AccountStatistic>, IHaveCustomMappings
    {
        public decimal DesiredAmount { get; set; }

        public decimal Available { get; set; }

        public decimal Equity { get; set; }

        public IEnumerable<string> Cards { get; set; }

        public string Card { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AccountStatistic, WithdrawViewModel>()
               .ForMember(x => x.Cards, opt =>
                   opt.MapFrom(x => x.User.Cards.Select(x => x.Number)));
        }
    }
}
