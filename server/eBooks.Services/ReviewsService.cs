﻿using eBooks.Database;
using eBooks.Database.Models;
using eBooks.Interfaces;
using eBooks.Models.Requests;
using eBooks.Models.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Http;

namespace eBooks.Services
{
    public class ReviewsService : BaseUserContextService<Review, ReviewsReq, ReviewsRes>, IReviewService
    {
        public ReviewsService(EBooksContext db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(db, mapper, httpContextAccessor, true)
        {
        }
    }
}
