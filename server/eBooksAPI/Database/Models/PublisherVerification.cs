﻿using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class PublisherVerification
    {
        public int VerificationId { get; set; }
        public int PublisherId { get; set; }
        public bool Verified { get; set; }
        public DateTime VerificationDate { get; set; }
        public int AdminId { get; set; }

        public virtual User Admin { get; set; } = null!;
        public virtual User Publisher { get; set; } = null!;
    }
}
