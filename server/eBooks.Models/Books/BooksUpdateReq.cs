﻿using Microsoft.AspNetCore.Http;

namespace eBooks.Models.Books
{
    public class BooksUpdateReq
    {
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public int? LanguageId { get; set; }
        public IFormFile? PdfFile { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
