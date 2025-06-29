﻿using eBooks.Models.Requests;
using eBooks.Models.Responses;
using eBooks.Models.Search;

namespace eBooks.Interfaces
{
    public interface ILanguagesService : IBaseCRUDService<LanguagesSearch, LanguagesReq, LanguagesReq, LanguagesRes>
    {
    }
}
