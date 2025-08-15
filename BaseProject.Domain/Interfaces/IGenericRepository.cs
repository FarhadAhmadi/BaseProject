using BaseProject.Domain.Entities;
using BaseProject.Shared.DTOs.Common;
using System.Linq.Expressions;

namespace BaseProject.Domain.Interfaces
{
    /// <summary>
    /// Generic repository interface supporting EF Core and Dapper operations.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        #region Dapper Methods

        #region Create
        // Typically Dapper is used for querying, so create methods are uncommon here.
        #endregion

        #region Read

        /// <summary>
        /// Retrieves paginated data using Dapper.
        /// </summary>
        Task<PaginatedList<TResult>> GetPaginatedAsync_Dapper<T, TResult>(
            string tableName,
            int pageIndex,
            int pageSize,
            Expression<Func<T, TResult>> selector,
            string? orderByColumn = "Id",
            bool ascending = true
        ) where T : BaseEntity;

        /// <summary>
        /// Gets an entity by ID using Dapper.
        /// </summary>
        Task<T> GetByIdAsync_Dapper(string tableName, object id);

        /// <summary>
        /// Gets a projected entity by ID using Dapper.
        /// </summary>
        Task<TResult?> GetByIdProjectedAsync_Dapper<TResult>(
            string tableName,
            object id,
            Expression<Func<T, TResult>> selector = null
        );

        /// <summary>
        /// Checks if an entity exists using Dapper.
        /// </summary>
        Task<bool> ExistsAsync_Dapper<T>(string tableName, string id);

        #endregion

        #region Update
        // Typically Dapper update methods are implemented in service layer
        #endregion

        #region Delete
        // Typically Dapper delete methods are implemented in service layer
        #endregion

        #endregion

        #region EF Core Methods

        #region Create

        /// <summary>
        /// Adds a single entity asynchronously.
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities asynchronously.
        /// </summary>
        Task AddRangeAsync(IEnumerable<T> entities);

        #endregion

        #region Read

        /// <summary>
        /// Checks if any entity satisfies a filter.
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Checks if any entity exists.
        /// </summary>
        Task<bool> ExistsAsync();

        /// <summary>
        /// Counts entities based on a filter.
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Counts all entities.
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// Retrieves a single projected entity with optional filters, includes, and sorting.
        /// </summary>
        Task<TResult?> GetFirstOrDefaultAsync<TResult>(
                    Expression<Func<T, bool>>? filter = null,
                    Func<IQueryable<T>, IQueryable<T>>? include = null,
                    Expression<Func<T, object>>? orderBy = null,
                    bool ascending = true,
                    Expression<Func<T, TResult>>? selector = null,
                    bool asNoTracking = true,
                    bool ignoreQueryFilters = true);

        /// <summary>
        /// Retrieves paginated data with optional filters, includes, and sorting.
        /// </summary>
        Task<PaginatedList<TResult>> GetPaginatedAsync<TResult>(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>> selector = null
        );

        #endregion

        #region Update

        /// <summary>
        /// Updates a single entity.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Updates multiple entities.
        /// </summary>
        void UpdateRange(IEnumerable<T> entities);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes a single entity.
        /// </summary>
        void Delete(T entity);

        /// <summary>
        /// Deletes multiple entities.
        /// </summary>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        Task Delete(object id);

        #endregion

        #endregion
    }
}
