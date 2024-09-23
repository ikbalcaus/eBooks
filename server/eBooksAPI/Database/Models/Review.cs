using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int? BookId { get; set; }
        public int? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? ReviewDate { get; set; }

        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}
