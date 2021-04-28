﻿namespace TopTrade.Web.Areas.User.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TopTrade.Data.Models;
    using TopTrade.Services.Data.User;

    public class UserCardViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserProfileService editProfileService;

        public UserCardViewComponent(
            UserManager<ApplicationUser> userManager,
            IUserProfileService editProfileService)
        {
            this.userManager = userManager;
            this.editProfileService = editProfileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var userDto = this.editProfileService.GetUserDataCardComponent(user);

            return this.View(userDto);
        }
    }
}