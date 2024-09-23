using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class BookFollow
    {
        public int FollowId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime? FollowDate { get; set; }

        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}
