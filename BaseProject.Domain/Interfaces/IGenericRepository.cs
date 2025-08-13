using BaseProject.Domain.Entities;
using BaseProject.Shared.DTOs.Common;
using System.Linq.Expressions;

namespace BaseProject.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

        #region Dapper

        Task<PaginatedList<TResult>> ToPaginationWithDapper<T, TResult>(
        string tableName,
        int pageIndex,
        int pageSize,
        Expression<Func<T, TResult>> selector,
        string? orderByColumn = "Id",
        bool ascending = true) where T : BaseEntity;

        Task<T> GetByIdAsyncWithDapper(string tableName, object id);
        Task<TResult?> GetByIdAsyncWithDapper<TResult>(
            string tableName,
            object id,
            Expression<Func<T, TResult>> selector = null);

        Task<bool> AnyAsyncWithDapper<T>(string tableName, string id);

        #endregion

        #region EF Core 

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task<bool> AnyAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync();
        Task<T> GetByIdAsync(object id);
        Task<TResult> GetSingleData<TResult>(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>> selector = null);

        Task<PaginatedList<TResult>> ToPagination<TResult>(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>> selector = null);
        Task<T?> FirstOrDefaultAsync(
           Expression<Func<T, bool>> filter,
           Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sort, bool ascending = true);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task Delete(object id);

        #endregion
    }
}
