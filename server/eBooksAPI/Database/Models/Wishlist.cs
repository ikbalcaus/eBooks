﻿using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class Wishlist
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime AddedDate { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
