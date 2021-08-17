namespace TopTrade.Services.Data.Tests.ServiceTests.Helpers
{
    using System.Reflection;

    using TopTrade.Services.Mapping;
    using TopTrade.Web.ViewModels.User.Profile;

    public class AutoMapperInitializer
    {
        public static void Init()
        {
            AutoMapperConfig.RegisterMappings(typeof(UserProfileCardViewModel).GetTypeInfo().Assembly);
        }
    }
}
