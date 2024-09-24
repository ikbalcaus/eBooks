using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using eBooksAPI.Database.Models;

namespace eBooksAPI.Database
{
    public partial class eBooksContext : DbContext
    {
        public eBooksContext()
        {
        }

        public eBooksContext(DbContextOptions<eBooksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccessRight> AccessRights { get; set; } = null!;
        public virtual DbSet<AuthToken> AuthTokens { get; set; } = null!;
        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookFollow> BookFollows { get; set; } = null!;
        public virtual DbSet<BookImage> BookImages { get; set; } = null!;
        public virtual DbSet<Favorite> Favorites { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Language> Languages { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<PublisherFollow> PublisherFollows { get; set; } = null!;
        public virtual DbSet<PublisherVerification> PublisherVerifications { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;
        public virtual DbSet<ReadingProgress> ReadingProgresses { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Wishlist> Wishlists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Database");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessRight>(entity =>
            {
                entity.HasOne(d => d.Book)
                    .WithMany(p => p.AccessRights)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccessRig__BookI__6477ECF3");
            });

            modelBuilder.Entity<AuthToken>(entity =>
            {
                entity.HasIndex(e => e.Token, "UQ__AuthToke__1EB4F81740141926")
                    .IsUnique();

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.LoggedInDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LoggedOutDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Token).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuthToken__UserI__46E78A0C");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Pdfpath)
                    .HasMaxLength(255)
                    .HasColumnName("PDFPath");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK__Books__GenreId__412EB0B6");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Books__Publisher__4222D4EF");

                entity.HasMany(d => d.Authors)
                    .WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BookAuthor",
                        l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BookAutho__Autho__619B8048"),
                        r => r.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BookAutho__BookI__60A75C0F"),
                        j =>
                        {
                            j.HasKey("BookId", "AuthorId").HasName("PK__BookAuth__6AED6DC4C740F08F");

                            j.ToTable("BookAuthors");
                        });
            });

            modelBuilder.Entity<BookFollow>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BookId })
                    .HasName("PK__BookFoll__7456C06CAD5DF74E");

                entity.Property(e => e.FollowDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookFollows)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookFollo__BookI__7B5B524B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookFollows)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookFollo__UserI__7A672E12");
            });

            modelBuilder.Entity<BookImage>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__BookImag__7516F70CE8AF77E9");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImagePath).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookImages)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookImage__BookI__4BAC3F29");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BookId })
                    .HasName("PK__Favorite__7456C06C6F502E9F");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorites__BookI__6C190EBB");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Favorites__UserI__6B24EA82");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Language__737584F661AFEB72")
                    .IsUnique();

                entity.Property(e => e.Abbreviation).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Notificat__BookI__02FC7413");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.NotificationPublishers)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK__Notificat__Publi__02084FDA");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__UserI__7F2BE32F");
            });

            modelBuilder.Entity<PublisherFollow>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PublisherId })
                    .HasName("PK__Publishe__B34E9BB690C87C6C");

                entity.Property(e => e.FollowDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherFollowPublishers)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Publi__76969D2E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PublisherFollowUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__UserI__75A278F5");
            });

            modelBuilder.Entity<PublisherVerification>(entity =>
            {
                entity.HasKey(e => e.VerificationId)
                    .HasName("PK__Publishe__306D4907C2217C07");

                entity.ToTable("PublisherVerification");

                entity.HasIndex(e => e.PublisherId, "UQ__Publishe__4C657FAAB5B31490")
                    .IsUnique();

                entity.Property(e => e.VerificationDate).HasColumnType("datetime");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.PublisherVerificationAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Admin__72C60C4A");

                entity.HasOne(d => d.Publisher)
                    .WithOne(p => p.PublisherVerificationPublisher)
                    .HasForeignKey<PublisherVerification>(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publisher__Publi__70DDC3D8");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BookId })
                    .HasName("PK__Purchase__7456C06C6C61D86D");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchases__BookI__5070F446");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchases__UserI__4F7CD00D");
            });

            modelBuilder.Entity<ReadingProgress>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BookId })
                    .HasName("PK__ReadingP__7456C06C7821962D");

                entity.ToTable("ReadingProgress");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ReadingProgresses)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReadingPr__BookI__68487DD7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReadingProgresses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReadingPr__UserI__6754599E");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.UserId })
                    .HasName("PK__Reviews__EC984EC363796F83");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviews__BookId__5441852A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviews__UserId__5535A963");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Roles__737584F6E392B466")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D105342F6284DC")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__RoleId__3B75D760");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.BookId })
                    .HasName("PK__Wishlist__7456C06C7A0EB259");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Wishlists__BookI__5AEE82B9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Wishlists__UserI__59FA5E80");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
