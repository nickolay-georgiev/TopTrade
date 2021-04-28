﻿namespace TopTrade.Data.Models.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using TopTrade.Data.Common.Models;

    public class Card : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        public string Number { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}