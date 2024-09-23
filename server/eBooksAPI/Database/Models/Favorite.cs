using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class Favorite
    {
        public int FavoriteId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime? AddedDate { get; set; }

        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}
