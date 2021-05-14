namespace TopTrade.Web.ViewModels.User.Profile
{
    using System.Linq;

    using AutoMapper;
    using TopTrade.Common;
    using TopTrade.Data.Models;
    using TopTrade.Data.Models.User.Enums;
    using TopTrade.Services.Mapping;

    public class UserProfileCardViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string AvatarUrl { get; set; }

        public string Username { get; set; }

        public string VerificationStatus { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserProfileCardViewModel>()
               .ForMember(x => x.VerificationStatus, opt =>
                   opt.MapFrom(x => x.Documents.Any() &&
                   x.Documents.All(d => d.VerificationStatus == VerificationDocumentStatus.Approved.ToString())
                   ? GlobalConstants.DisplayedStatusVerified : GlobalConstants.DisplayedStatusNotVerified));
        }
    }
}
