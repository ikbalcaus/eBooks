﻿using System;
using System.Collections.Generic;

namespace eBooks.Database.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int RoleId { get; set; }

    public string? StripeAccountId { get; set; }

    public bool IsEmailVerified { get; set; }

    public string? VerificationToken { get; set; }

    public DateTime? TokenExpiry { get; set; }

    public virtual ICollection<AccessRight> AccessRights { get; set; } = new List<AccessRight>();

    public virtual ICollection<Book> BookPublishers { get; set; } = new List<Book>();

    public virtual ICollection<Book> BookReviewedBies { get; set; } = new List<Book>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Notification> NotificationPublishers { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationUsers { get; set; } = new List<Notification>();

    public virtual ICollection<PublisherFollow> PublisherFollowPublishers { get; set; } = new List<PublisherFollow>();

    public virtual ICollection<PublisherFollow> PublisherFollowUsers { get; set; } = new List<PublisherFollow>();

    public virtual ICollection<PublisherVerification> PublisherVerificationAdmins { get; set; } = new List<PublisherVerification>();

    public virtual PublisherVerification? PublisherVerificationPublisher { get; set; }

    public virtual ICollection<Purchase> PurchasePublishers { get; set; } = new List<Purchase>();

    public virtual ICollection<Purchase> PurchaseUsers { get; set; } = new List<Purchase>();

    public virtual ICollection<ReadingProgress> ReadingProgresses { get; set; } = new List<ReadingProgress>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
