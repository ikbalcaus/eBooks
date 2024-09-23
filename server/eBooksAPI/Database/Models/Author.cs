using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int AuthorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
