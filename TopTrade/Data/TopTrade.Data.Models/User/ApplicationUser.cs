// ReSharper disable VirtualMemberCallInConstructor
namespace TopTrade.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;
    using TopTrade.Data.Common.Models;
    using TopTrade.Data.Models.User;
    using TopTrade.Data.Models.User.Enums;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            this.Cards = new HashSet<Card>();
            this.Trades = new HashSet<Trade>();
            this.Deposits = new HashSet<Deposit>();
            this.Withdraws = new HashSet<Withdraw>();
            this.Documents = new HashSet<VerificationDocument>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int ZipCode { get; set; }

        public string AvatarUrl { get; set; }

        public int? WatchlistId { get; set; }

        public virtual Watchlist Watchlist { get; set; }

        public virtual UserProfileVerificationStatus ProfileStatus { get; set; }

        public virtual ICollection<Card> Cards { get; set; }

        public virtual ICollection<Deposit> Deposits { get; set; }

        public virtual ICollection<Withdraw> Withdraws { get; set; }

        public virtual ICollection<Trade> Trades { get; set; }

        public virtual ICollection<VerificationDocument> Documents { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
