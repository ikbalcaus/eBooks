﻿using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using eBooks.Database.Models;
using Microsoft.AspNetCore.Http;
using eBooks.Database;
using MapsterMapper;
using eBooks.Models.Exceptions;
using eBooks.Models.Responses;

namespace eBooks.Services
{
    public class Helpers
    {
        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            var hasMinimum8Chars = password.Length >= 8;
            var hasUpperChar = Regex.IsMatch(password, "[A-Z]");
            var hasLowerChar = Regex.IsMatch(password, "[a-z]");
            var hasDigit = Regex.IsMatch(password, "[0-9]");
            return hasMinimum8Chars && hasUpperChar && hasLowerChar && hasDigit;
        }

        public static string GenerateSalt(int size = 32)
        {
            var saltBytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string GenerateHash(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = password + salt;
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static List<BookImageRes> UploadImages(EBooksContext db, IMapper mapper, int id, List<IFormFile> files)
        {
            var folderPath = Path.Combine("wwwroot", "images", $"book_{id.ToString()}");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            var uploadedImages = new List<BookImageRes>();
            foreach (var file in files)
            {
                if (file.ContentType != "image/jpeg" && file.ContentType != "image/png") throw new ExceptionBadRequest("Invalid file type. Only JPEG and PNG are allowed.");
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var bookImage = new BookImage
                {
                    BookId = id,
                    ImagePath = Path.Combine("images", $"book_{id.ToString()}", fileName).Replace("\\", "/"),
                    ModifiedAt = DateTime.UtcNow
                };
                db.Set<BookImage>().Add(bookImage);
                var image = mapper.Map<BookImageRes>(bookImage);
                uploadedImages.Add(image);
            }
            db.SaveChanges();
            return uploadedImages;
        }

        public static BooksRes UploadPdfFile(EBooksContext db, IMapper mapper, Book entity, IFormFile file)
        {
            if (file.ContentType != "application/pdf") throw new ExceptionBadRequest("Book created, file must be PDF");
            if (entity == null) return null;
            var folderPath = Path.Combine("wwwroot", "pdfs");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!string.IsNullOrEmpty(entity.PdfPath))
            {
                var oldFilePath = Path.Combine("wwwroot", entity.PdfPath.TrimStart('/'));
                if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
            }
            var fileName = $"{Guid.NewGuid()}.pdf";
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            entity.PdfPath = $"/pdfs/{fileName}";
            db.SaveChanges();
            return mapper.Map<BooksRes>(entity);
        }

        public static decimal? CalculateDiscountedPrice(decimal? price, int? discountPercentage, DateTime? discountStart, DateTime? discountEnd)
        {
            if (price == null) return null;
            if (discountPercentage.HasValue && discountStart.HasValue && discountEnd.HasValue)
            {
                var now = DateTime.UtcNow;
                if (now >= discountStart.Value && now <= discountEnd.Value)
                {
                    decimal discountFactor = (100 - discountPercentage.Value) / 100m;
                    return Math.Round(price.Value * discountFactor, 2);
                }
            }
            return price;
        }
    }
}
