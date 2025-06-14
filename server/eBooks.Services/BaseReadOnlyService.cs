﻿using eBooks.Database;
using eBooks.Models.Responses;
using eBooks.Models.Exceptions;
using eBooks.Models.Search;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eBooks.Services
{
    public abstract class BaseReadOnlyService<TEntity, TSearch, TResponse> : IBaseReadOnlyService<TSearch, TResponse>
        where TEntity : class
        where TSearch : BaseSearch
        where TResponse : class
    {
        protected EBooksContext _db;
        protected IMapper _mapper;

        public BaseReadOnlyService(EBooksContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<TResponse>> GetPaged(TSearch search)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            query = AddIncludes(query, search);
            query = AddFilters(query, search);
            int count = await query.CountAsync();
            if (search?.Page.HasValue == true && search?.PageSize.HasValue == true && search.Page.Value > 0)
                query = query.Skip((search.Page.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            var list = await query.ToListAsync();
            var result = new List<TResponse>();
            result = _mapper.Map(list, result);
            PagedResult<TResponse> pagedResult = new PagedResult<TResponse>
            {
                ResultList = result,
                Count = count
            };
            return pagedResult;
        }

        public virtual async Task<TResponse> GetById(int id)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            query = AddIncludes(query);
            var idProperty = typeof(TEntity).GetProperties().FirstOrDefault(x => x.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase));
            var entity = await query.FirstOrDefaultAsync(x => EF.Property<int>(x, idProperty.Name) == id);
            if (entity == null)
                throw new ExceptionNotFound();
            return _mapper.Map<TResponse>(entity);
        }

        public virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query, TSearch? search = null)
        {
            return query;
        }

        public virtual IQueryable<TEntity> AddFilters(IQueryable<TEntity> query, TSearch search)
        {
            return query;
        }
    }
}
