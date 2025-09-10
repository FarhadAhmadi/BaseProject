using BaseProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TransactionException = BaseProject.Application.Common.Exceptions.TransactionException;
using BaseProject.Infrastructure.Persistence.Repositories;

namespace BaseProject.Infrastructure.Persistence
{
    /// <summary>
    /// Implements the Unit of Work pattern to manage repositories and database transactions.
    /// Ensures all repository operations within a transaction are committed or rolled back together.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SqlDapperContext _dapperContext;

        #region Repositories

        /// <summary>
        /// Repository for managing user entities.
        /// </summary>
        public IUserRepository Users { get; }

        /// <summary>
        /// Repository for managing refresh tokens.
        /// </summary>
        public IRefreshTokenRepository RefreshTokens { get; }

        /// <summary>
        /// Repository for managing media files.
        /// </summary>
        public IMediaRepository Media { get; }

        /// <summary>
        /// Repository for managing forgot password requests.
        /// </summary>
        /// 
        public IForgotPasswordRepository ForgotPasswords { get; }

        #region Authorize
        public IPermissionRecordRepository PermissionRecords{ get; }
        public IPermissionActionRepository PermissionActions { get; }
        public IRolePermissionActionRepository RolePermissionActions{ get; }
        public IUserPermissionActionRepository UserPermissionActions { get; }

        #endregion


        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dbContext">EF Core DbContext for database operations.</param>
        /// <param name="dapperContext">Dapper context for direct SQL operations.</param>
        public UnitOfWork(ApplicationDbContext dbContext, SqlDapperContext dapperContext)
        {
            _dbContext = dbContext;
            _dapperContext = dapperContext;

            Users = new UserRepository(_dbContext, _dapperContext);
            RefreshTokens = new RefreshTokenRepository(_dbContext, _dapperContext);
            Media = new MediaRepository(_dbContext, _dapperContext);
            ForgotPasswords = new ForgotPasswordRepository(_dbContext, _dapperContext);

            #region Authorize

            PermissionRecords = new PermissionRecordRepository(_dbContext, _dapperContext);
            PermissionActions = new PermissionActionRepository(_dbContext, _dapperContext);
            RolePermissionActions = new RolePermissionActionRepository(_dbContext, _dapperContext);
            UserPermissionActions = new UserPermissionActionRepository(_dbContext, _dapperContext);

            #endregion

        }

        #region Save Changes

        /// <summary>
        /// Saves all changes made in the current DbContext to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Saves all changes made in the current DbContext to the database asynchronously.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }


        #endregion

        #region Transaction Handling

        /// <summary>
        /// Executes a synchronous operation within a database transaction.
        /// Commits if successful, rolls back if an exception occurs.
        /// </summary>
        /// <param name="action">Synchronous action to execute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="TransactionException">Throws when transaction fails.</exception>
        public async Task ExecuteInTransactionAsync(Action action, CancellationToken cancellationToken)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    action();
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw TransactionException.TransactionNotExecuteException(ex);
                }
            });
        }

        /// <summary>
        /// Executes an asynchronous operation within a database transaction.
        /// Commits if successful, rolls back if an exception occurs.
        /// </summary>
        /// <param name="action">Asynchronous action to execute.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <exception cref="TransactionException">Throws when transaction fails.</exception>
        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await action();
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw TransactionException.TransactionNotExecuteException(ex);
                }
            });
        }

        #endregion
    }
}
