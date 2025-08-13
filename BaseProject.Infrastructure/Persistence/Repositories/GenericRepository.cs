using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using BaseProject.Shared.DTOs.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        private readonly SqlDapperContext _dapperContext;

        public GenericRepository(ApplicationDbContext context, SqlDapperContext dapperContext)
        {
            _dbSet = context.Set<T>();
            _dapperContext = dapperContext ?? throw new ArgumentNullException(nameof(dapperContext)); // Ensure it's not null
        }

        #region Dapper Methods

        public async Task<PaginatedList<TResult>> ToPaginationWithDapper<T, TResult>(
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
            )).ToArray(); // Convert to array

            return MapToPaginatedList<TResult>(result, pageIndex, pageSize);

        }

        #region ToPaginatedList Private Methods

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

        public async Task<T> GetByIdAsyncWithDapper(string tableName, object id)
        {
            using var connection = _dapperContext.CreateConnection();

            string sql = $"SELECT * FROM {tableName} WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<TResult?> GetByIdAsyncWithDapper<TResult>(
            string tableName,
            object id,
            Expression<Func<T, TResult>> selector = null)
        {
            try
            {
                using var connection = _dapperContext.CreateConnection();

                // Extract selected columns from the selector expression
                var selectedColumns = ExtracterHelper.ExtractSelectedColumns(selector);

                // Generate the column selection dynamically
                string selectedColumnsSql = string.Join(", ", selectedColumns);

                // SQL query
                string sql = $"SELECT {selectedColumnsSql} FROM {tableName} WHERE Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<TResult>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                var a = 1;
                throw;
            }
        }

        public async Task<bool> AnyAsyncWithDapper<T>(string tableName, string id)
        {
            using var connection = _dapperContext.CreateConnection();

            // Build the full SQL query
            string sql = $"SELECT COUNT(1) FROM {tableName} WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            // Execute the query with the parameters
            return await connection.ExecuteScalarAsync<int>(sql, parameters) > 0;
        }




        #endregion

        #region EF Core Methods

        #region Add

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion

        #region  Read

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbSet.AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TResult> GetSingleData<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<T, TResult>> selector = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Default ordering by Id if no orderBy is provided
            orderBy ??= x => EF.Property<object>(x, "Id");

            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            var projectedQuery = query.Select(selector);

            // Retrieve the first matching result or default if not found
            var result = await projectedQuery.FirstOrDefaultAsync();

            return result;
        }

        public async Task<PaginatedList<TResult>> ToPagination<TResult>(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>> selector = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            orderBy ??= x => EF.Property<object>(x, "Id");

            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            var projectedQuery = query.Select(selector);

            var result = await PaginatedList<TResult>.ToPagedList(projectedQuery, pageIndex, pageSize);

            return result;
        }

        public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet.IgnoreQueryFilters().AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> sort, bool ascending = true)
        {
            var query = _dbSet.IgnoreQueryFilters()
                              .AsNoTracking()
                              .Where(filter);

            query = ascending ? query.OrderBy(sort) : query.OrderByDescending(sort);

            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region Update & delete

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task Delete(object id)
        {
            T entity = await GetByIdAsync(id);
            Delete(entity);
        }

        #endregion

        #endregion
    }
}
