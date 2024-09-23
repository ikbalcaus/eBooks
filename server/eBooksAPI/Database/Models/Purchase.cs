using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class Purchase
    {
        public int PurchaseId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? TotalPrice { get; set; }

        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}
