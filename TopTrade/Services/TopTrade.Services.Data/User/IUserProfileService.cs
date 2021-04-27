namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User.Models;
    using TopTrade.Web.ViewModels.User;

    public interface IUserProfileService
    {
        Task EditProfileAsync(ApplicationUser user, EditProfileViewModel input);

        UserProfileDto GetUserDataProfilePage(ApplicationUser user);

        UserCardDto GetUserDataCardComponent(ApplicationUser user);
    }
}
