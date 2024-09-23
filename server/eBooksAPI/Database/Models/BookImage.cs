using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class BookImage
    {
        public int ImageId { get; set; }
        public int? BookId { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? AddedDate { get; set; }

        public virtual Book? Book { get; set; }
    }
}
