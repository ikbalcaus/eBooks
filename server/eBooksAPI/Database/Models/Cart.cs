using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime? AddedDate { get; set; }

        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}
