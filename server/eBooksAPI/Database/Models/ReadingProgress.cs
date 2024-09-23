using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class ReadingProgress
    {
        public int ProgressId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public int? CurrentPage { get; set; }
        public DateTime? LastReadDate { get; set; }

        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}
