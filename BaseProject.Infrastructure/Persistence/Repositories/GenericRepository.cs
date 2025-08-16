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

        public GenericRepository(ApplicationDbContext context, SqlDapperContext dapperContext)
        {
            _dbSet = context.Set<T>();
            _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext));
        }

        #region Dapper Methods

        #region Read

        public async Task<PaginatedList<TResult>> GetPaginatedAsync_Dapper<TResult>(
            string tableName,
            int pageIndex = 0,
            int pageSize = 10,
            Expression<Func<T, TResult>>? selector = null,
            string? orderByColumn = "Id",
            bool ascending = true)
        {
            ValidateTableAndColumn(tableName, orderByColumn);

            try
            {
                using var connection = _dapperContext.CreateConnection();

                string selectedColumns = selector != null
                    ? string.Join(", ", ExtractSelectedColumns(selector))
                    : "*";

                var sql = $@"
                    WITH FilteredData AS (
                        SELECT {selectedColumns}, COUNT(*) OVER() AS TotalCount
                        FROM {tableName}
                    )
                    SELECT {selectedColumns}, TotalCount FROM FilteredData
                    ORDER BY {orderByColumn} {(ascending ? "ASC" : "DESC")}
                    OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;";

                var parameters = new DynamicParameters();
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@PageSize", pageSize);

                var result = (await connection.QueryAsync<TResult, int, (TResult, int)>(
                    sql,
                    (data, totalCount) => (data, totalCount),
                    parameters,
                    splitOn: "TotalCount"
                )).ToArray();

                var items = result.Select(r => r.Item1).ToList();
                int totalCount = result.Any() ? result.First().Item2 : 0;

                return new PaginatedList<TResult>(items, totalCount, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Dapper paginated query failed: {ex.Message}", ex);
            }
        }

        public async Task<T?> GetByIdAsync_Dapper(string tableName, object id)
        {
            ValidateTableAndColumn(tableName, "Id");

            try
            {
                using var connection = _dapperContext.CreateConnection();
                string sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Dapper GetById failed: {ex.Message}", ex);
            }
        }

        public async Task<TResult?> GetByIdProjectedAsync_Dapper<TResult>(
            string tableName,
            object id,
            Expression<Func<T, TResult>>? selector = null) // optional
        {
            ValidateTableAndColumn(tableName, "Id");

            try
            {
                using var connection = _dapperContext.CreateConnection();

                string sql = selector != null
                    ? $"SELECT {string.Join(", ", ExtractSelectedColumns(selector))} FROM {tableName} WHERE Id = @Id"
                    : $"SELECT * FROM {tableName} WHERE Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<TResult>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Dapper GetByIdProjected failed: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExistsAsync_Dapper(string tableName, object id)
        {
            ValidateTableAndColumn(tableName, "Id");

            try
            {
                using var connection = _dapperContext.CreateConnection();
                string sql = $"SELECT COUNT(1) FROM {tableName} WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                return await connection.ExecuteScalarAsync<int>(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Dapper ExistsAsync failed: {ex.Message}", ex);
            }
        }

        #region Dapper Private Helpers

        private List<string> ExtractSelectedColumns<TResult>(Expression<Func<T, TResult>> selector)
        {
            return ExtracterHelper.ExtractSelectedColumns(selector);
        }

        private void ValidateTableAndColumn(string tableName, string columnName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));
        }

        #endregion

        #endregion

        #endregion

        #region EF Core Methods

        #region Create

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        #endregion

        #region Read

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter) => await _dbSet.AnyAsync(filter);
        public async Task<bool> ExistsAsync() => await _dbSet.AnyAsync();
        public async Task<int> CountAsync(Expression<Func<T, bool>> filter) =>
            filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);
        public async Task<int> CountAsync() => await _dbSet.CountAsync();

        public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public async Task<TResult?> GetFirstOrDefaultAsync<TResult>(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>>? selector = null, // optional
            bool asNoTracking = true,
            bool ignoreQueryFilters = true)
        {
            IQueryable<T> query = _dbSet;
            if (asNoTracking) query = query.AsNoTracking();
            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();
            if (include != null) query = include(query);
            if (filter != null) query = query.Where(filter);
            if (orderBy != null) query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            return selector != null
                ? await query.Select(selector).FirstOrDefaultAsync()
                : (TResult?)(object?)await query.FirstOrDefaultAsync();
        }

        public async Task<PaginatedList<TResult>> GetPaginatedAsync<TResult>(
            int pageIndex = 0,
            int pageSize = 10,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>>? selector = null) // optional
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (include != null) query = include(query);
            if (filter != null) query = query.Where(filter);
            orderBy ??= x => EF.Property<object>(x, "Id");
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            if (selector != null)
            {
                var projectedQuery = query.Select(selector);
                return await PaginatedList<TResult>.ToPagedList(projectedQuery, pageIndex, pageSize);
            }
            else
            {
                var projectedQuery = query.Cast<TResult>();
                return await PaginatedList<TResult>.ToPagedList(projectedQuery, pageIndex, pageSize);
            }
        }

        #endregion

        #region Update

        public void Update(T entity) => _dbSet.Update(entity);
        public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

        #endregion

        #region Delete

        public void Delete(T entity) => _dbSet.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public async Task Delete(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) Delete(entity);
        }

        #endregion

        #endregion
    }
}
