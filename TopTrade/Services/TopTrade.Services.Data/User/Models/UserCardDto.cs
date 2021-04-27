namespace TopTrade.Services.Data.User.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserCardDto
    {
        public string AvatarUrl { get; set; }

        public string Username { get; set; }

        public string VerificationStatus { get; set; }
    }
}
