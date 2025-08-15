using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using BaseProject.Shared.DTOs.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Generic repository supporting EF Core and Dapper operations for any entity type.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        private readonly SqlDapperContext _dapperContext;

        /// <summary>
        /// Constructor for GenericRepository.
        /// </summary>
        /// <param name="context">EF Core DbContext</param>
        /// <param name="dapperContext">Dapper context</param>
        public GenericRepository(ApplicationDbContext context, SqlDapperContext dapperContext)
        {
            _dbSet = context.Set<T>();
            _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
        }

        #region Dapper Methods

        #region Read

        /// <summary>
        /// Retrieves paginated data from the database using Dapper.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TResult">Projected type</typeparam>
        /// <param name="tableName">Database table name</param>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="selector">Projection selector</param>
        /// <param name="orderByColumn">Column to order by</param>
        /// <param name="ascending">Sort direction</param>
        /// <returns>Paginated list of projected items</returns>
        public async Task<PaginatedList<TResult>> GetPaginatedAsync_Dapper<T, TResult>(
            string tableName,
            int pageIndex,
            int pageSize,
            Expression<Func<T, TResult>> selector,
            string? orderByColumn = "Id",
            bool ascending = true) where T : BaseEntity
        {
            using var connection = _dapperContext.CreateConnection();
            var selectedColumns = ExtractSelectedColumns(selector);
            var sql = BuildPaginatedListQuery(tableName, selectedColumns, orderByColumn, ascending);
            var parameters = CreateParameters(pageIndex, pageSize);

            var result = (await connection.QueryAsync<TResult, int, (TResult, int)>(
                sql,
                (data, totalCount) => (data, totalCount),
                parameters,
                splitOn: "TotalCount"
            )).ToArray();

            return MapToPaginatedList<TResult>(result, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets an entity by ID using Dapper.
        /// </summary>
        public async Task<T> GetByIdAsync_Dapper(string tableName, object id)
        {
            using var connection = _dapperContext.CreateConnection();
            string sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        /// <summary>
        /// Gets a projected entity by ID using Dapper.
        /// </summary>
        public async Task<TResult?> GetByIdProjectedAsync_Dapper<TResult>(
            string tableName,
            object id,
            Expression<Func<T, TResult>> selector = null)
        {
            using var connection = _dapperContext.CreateConnection();
            var selectedColumns = ExtracterHelper.ExtractSelectedColumns(selector);
            string sql = $"SELECT {string.Join(", ", selectedColumns)} FROM {tableName} WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<TResult>(sql, new { Id = id });
        }

        /// <summary>
        /// Checks if an entity exists in the database using Dapper.
        /// </summary>
        public async Task<bool> ExistsAsync_Dapper<T>(string tableName, string id)
        {
            using var connection = _dapperContext.CreateConnection();
            string sql = $"SELECT COUNT(1) FROM {tableName} WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            return await connection.ExecuteScalarAsync<int>(sql, parameters) > 0;
        }

        #region Dapper Private Helpers

        private List<string> ExtractSelectedColumns<T, TResult>(Expression<Func<T, TResult>> selector)
        {
            return ExtracterHelper.ExtractSelectedColumns(selector);
        }

        private string BuildPaginatedListQuery(string tableName, List<string> selectedColumns, string orderByColumn, bool ascending)
        {
            string sortDirection = ascending ? "ASC" : "DESC";
            return $@"
                WITH FilteredData AS (
                    SELECT {string.Join(", ", selectedColumns)}, COUNT(*) OVER() AS TotalCount
                    FROM {tableName}
                )
                SELECT {string.Join(", ", selectedColumns)}, TotalCount FROM FilteredData
                ORDER BY {orderByColumn} {sortDirection}
                OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;";
        }

        private DynamicParameters CreateParameters(int pageIndex, int pageSize)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@PageSize", pageSize);
            return parameters;
        }

        private PaginatedList<TResult> MapToPaginatedList<TResult>((TResult, int)[] result, int pageIndex, int pageSize)
        {
            var items = result.Select(r => r.Item1).ToList();
            int totalCount = result.Any() ? result.First().Item2 : 0;
            return new PaginatedList<TResult>(items, totalCount, pageIndex, pageSize);
        }

        #endregion

        #endregion

        #endregion

        #region EF Core Methods

        #region Create

        /// <summary>
        /// Adds a single entity to the database asynchronously.
        /// </summary>
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        /// <summary>
        /// Adds multiple entities to the database asynchronously.
        /// </summary>
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        #endregion

        #region Read

        /// <summary>
        /// Checks if any entity satisfies a given filter.
        /// </summary>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter) => await _dbSet.AnyAsync(filter);

        /// <summary>
        /// Checks if any entity exists.
        /// </summary>
        public async Task<bool> ExistsAsync() => await _dbSet.AnyAsync();

        /// <summary>
        /// Counts entities satisfying a filter.
        /// </summary>
        public async Task<int> CountAsync(Expression<Func<T, bool>> filter) =>
            filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);

        /// <summary>
        /// Counts all entities in the table.
        /// </summary>
        public async Task<int> CountAsync() => await _dbSet.CountAsync();

        /// <summary>
        /// Gets an entity by its primary key.
        /// </summary>
        public async Task<T> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        /// <summary>
        /// Retrieves a single entity (or projected type) matching a filter with optional includes and ordering.
        /// </summary>
        public async Task<TResult?> GetFirstOrDefaultAsync<TResult>(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>>? selector = null,
            bool asNoTracking = true,
            bool ignoreQueryFilters = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking) query = query.AsNoTracking();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            if (include != null) query = include(query);
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            if (selector != null) return await query.Select(selector).FirstOrDefaultAsync();
            return (TResult?)(object?)await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves paginated data with optional filter, include, and ordering.
        /// </summary>
        public async Task<PaginatedList<TResult>> GetPaginatedAsync<TResult>(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>> selector = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (include != null) query = include(query);
            if (filter != null) query = query.Where(filter);
            orderBy ??= x => EF.Property<object>(x, "Id");
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            var projectedQuery = query.Select(selector);
            return await PaginatedList<TResult>.ToPagedList(projectedQuery, pageIndex, pageSize);
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates a single entity in the database.
        /// </summary>
        public void Update(T entity) => _dbSet.Update(entity);

        /// <summary>
        /// Updates multiple entities in the database.
        /// </summary>
        public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes a single entity from the database.
        /// </summary>
        public void Delete(T entity) => _dbSet.Remove(entity);

        /// <summary>
        /// Deletes multiple entities from the database.
        /// </summary>
        public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        /// <summary>
        /// Deletes an entity by its primary key.
        /// </summary>
        public async Task Delete(object id)
        {
            var entity = await GetByIdAsync(id);
            Delete(entity);
        }

        #endregion

        #endregion
    }
}
