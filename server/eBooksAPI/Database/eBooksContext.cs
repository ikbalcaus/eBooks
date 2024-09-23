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
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Favorite> Favorites { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=eBooks;Trusted_Connection=true;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessRight>(entity =>
            {
                entity.HasOne(d => d.Book)
                    .WithMany(p => p.AccessRights)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__AccessRig__BookI__656C112C");
            });

            modelBuilder.Entity<AuthToken>(entity =>
            {
                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.LoggedInDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Token).HasMaxLength(255);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuthTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__AuthToken__UserI__44FF419A");
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
                    .HasConstraintName("FK__Books__GenreId__403A8C7D");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK__Books__Publisher__412EB0B6");

                entity.HasMany(d => d.Authors)
                    .WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BookAuthor",
                        l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BookAutho__Autho__628FA481"),
                        r => r.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__BookAutho__BookI__619B8048"),
                        j =>
                        {
                            j.HasKey("BookId", "AuthorId").HasName("PK__BookAuth__6AED6DC4B627DD2B");

                            j.ToTable("BookAuthors");
                        });
            });

            modelBuilder.Entity<BookFollow>(entity =>
            {
                entity.HasKey(e => e.FollowId)
                    .HasName("PK__BookFoll__2CE810AEDF9233BA");

                entity.Property(e => e.FollowDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookFollows)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__BookFollo__BookI__7C4F7684");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookFollows)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__BookFollo__UserI__7B5B524B");
            });

            modelBuilder.Entity<BookImage>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__BookImag__7516F70C67EDBE97");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImagePath).HasMaxLength(255);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookImages)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__BookImage__BookI__47DBAE45");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Cart__BookId__5BE2A6F2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Cart__UserId__5AEE82B9");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Favorites__BookI__6E01572D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Favorites__UserI__6D0D32F4");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.GenreName).HasMaxLength(100);
            });

            modelBuilder.Entity<PublisherFollow>(entity =>
            {
                entity.HasKey(e => e.FollowId)
                    .HasName("PK__Publishe__2CE810AE3797B2C8");

                entity.Property(e => e.FollowDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherFollowPublishers)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK__Publisher__Publi__778AC167");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PublisherFollowUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Publisher__UserI__76969D2E");
            });

            modelBuilder.Entity<PublisherVerification>(entity =>
            {
                entity.HasKey(e => e.VerificationId)
                    .HasName("PK__Publishe__306D4907439E487F");

                entity.ToTable("PublisherVerification");

                entity.Property(e => e.VerificationDate).HasColumnType("datetime");

                entity.Property(e => e.Verified).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.PublisherVerificationAdmins)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__Publisher__Admin__73BA3083");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherVerificationPublishers)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK__Publisher__Publi__71D1E811");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.PurchaseDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Purchases__BookI__4CA06362");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Purchases__UserI__4BAC3F29");
            });

            modelBuilder.Entity<ReadingProgress>(entity =>
            {
                entity.HasKey(e => e.ProgressId)
                    .HasName("PK__ReadingP__BAE29CA5DA150D4E");

                entity.ToTable("ReadingProgress");

                entity.Property(e => e.LastReadDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ReadingProgresses)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__ReadingPr__BookI__693CA210");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReadingProgresses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ReadingPr__UserI__68487DD7");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Reviews__BookId__5070F446");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Reviews__UserId__5165187F");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D1053433BB889D")
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
                    .HasConstraintName("FK__Users__RoleId__3A81B327");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Wishlists__BookI__571DF1D5");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Wishlists__UserI__5629CD9C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
