using System;
using System.Collections.Generic;

namespace eBooksAPI.Database.Models
{
    public partial class User
    {
        public User()
        {
            AuthTokens = new HashSet<AuthToken>();
            BookFollows = new HashSet<BookFollow>();
            Books = new HashSet<Book>();
            Carts = new HashSet<Cart>();
            Favorites = new HashSet<Favorite>();
            PublisherFollowPublishers = new HashSet<PublisherFollow>();
            PublisherFollowUsers = new HashSet<PublisherFollow>();
            PublisherVerificationAdmins = new HashSet<PublisherVerification>();
            PublisherVerificationPublishers = new HashSet<PublisherVerification>();
            Purchases = new HashSet<Purchase>();
            ReadingProgresses = new HashSet<ReadingProgress>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public int? RoleId { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<AuthToken> AuthTokens { get; set; }
        public virtual ICollection<BookFollow> BookFollows { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<PublisherFollow> PublisherFollowPublishers { get; set; }
        public virtual ICollection<PublisherFollow> PublisherFollowUsers { get; set; }
        public virtual ICollection<PublisherVerification> PublisherVerificationAdmins { get; set; }
        public virtual ICollection<PublisherVerification> PublisherVerificationPublishers { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
