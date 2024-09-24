using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class Book
    {
        public Book()
        {
            AccessRights = new HashSet<AccessRight>();
            BookFollows = new HashSet<BookFollow>();
            BookImages = new HashSet<BookImage>();
            Favorites = new HashSet<Favorite>();
            Notifications = new HashSet<Notification>();
            Purchases = new HashSet<Purchase>();
            ReadingProgresses = new HashSet<ReadingProgress>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
            Authors = new HashSet<Author>();
        }

        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? GenreId { get; set; }
        public decimal Price { get; set; }
        public int TotalPages { get; set; }
        public string? Pdfpath { get; set; }
        public int PublisherId { get; set; }
        public DateTime AddedDate { get; set; }

        public virtual Genre? Genre { get; set; }
        public virtual User Publisher { get; set; } = null!;
        public virtual ICollection<AccessRight> AccessRights { get; set; }
        public virtual ICollection<BookFollow> BookFollows { get; set; }
        public virtual ICollection<BookImage> BookImages { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}
