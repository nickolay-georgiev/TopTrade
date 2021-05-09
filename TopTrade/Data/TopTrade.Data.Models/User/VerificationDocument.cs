namespace TopTrade.Data.Models.User
{
    using System;

    using TopTrade.Data.Common.Models;

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
