namespace TopTrade.Services.Data.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User.Models;
    using TopTrade.Web.ViewModels.User;

    public class EditProfileService : IEditProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public EditProfileService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task EditProfileAsync(ApplicationUser user, EditProfileViewModel input)
        {
            user.FirstName = input.FirstName;
            user.MiddleName = input.MiddleName;
            user.LastName = input.LastName;
            user.Address = input.Address;
            user.City = input.City;
            user.Country = input.Country;
            user.ZipCode = input.ZipCode;
            user.AboutMe = input.AboutMe;

            await this.userManager.UpdateAsync(user);
        }

        public UserProfileDto GetUserData(ApplicationUser user)
        {
            var userDto = new UserProfileDto
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                ZipCode = user.ZipCode,
                AboutMe = user.AboutMe,
                AvatarUrl = user.AvatarUrl,
            };

            return userDto;
        }
    }
}
