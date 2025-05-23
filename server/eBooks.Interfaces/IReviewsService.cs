﻿using eBooks.Models.Requests;
using eBooks.Models.Responses;

namespace eBooks.Interfaces
{
    public interface IReviewService : IBaseUserContextService<ReviewsReq, ReviewsRes>
    {
    }
}
