namespace TopTrade.Web.ViewModels.Administration
{
    using AutoMapper;
    using TopTrade.Data.Models;
    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.AccountManager;

    public class UserInUsersPageViewModel : UserInPageViewModel, IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string AvatarUrl { get; set; }

        public string ProfileStatus { get; set; }

        public decimal Profit { get; set; }

        public decimal Available { get; set; }

        public decimal Allocated { get; set; }

        public decimal UserEquity => this.Available + this.Profit + this.Allocated;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserInUsersPageViewModel>()
               .ForMember(x => x.Profit, opt =>
                   opt.MapFrom(x => x.AccountStatistic.Profit))
               .ForMember(x => x.Available, opt =>
                   opt.MapFrom(x => x.AccountStatistic.Available))
                .ForMember(x => x.Allocated, opt =>
                   opt.MapFrom(x => x.AccountStatistic.TotalAllocated));
        }
    }
}
