namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using TopTrade.Data.Common.Models;
    using TopTrade.Data.Models.User.Enums;

    public class VerificationDocument : BaseDeletableModel<string>
    {
        public VerificationDocument()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string DocumentUrl { get; set; }

        public string VerificationStatus { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
