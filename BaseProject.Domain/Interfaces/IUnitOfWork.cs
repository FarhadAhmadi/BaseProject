using System;
using System.Threading;
using System.Threading.Tasks;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Domain.Interfaces
{
    /// <summary>
    /// Defines a Unit of Work for managing multiple repositories and database transactions.
    /// Ensures that all operations within a transaction are committed or rolled back together.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Repositories

        /// <summary>
        /// Repository for managing user entities.
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Repository for managing refresh tokens.
        /// </summary>
        IRefreshTokenRepository RefreshTokens { get; }

        /// <summary>
        /// Repository for managing media files.
        /// </summary>
        IMediaRepository Media { get; }

        /// <summary>
        /// Repository for managing forgot password requests.
        /// </summary>
        IForgotPasswordRepository ForgotPasswords { get; }

        /// <summary>
        /// Repository for managing PermissionRecord
        /// </summary>
        IPermissionRecordRepository PermissionRecordRepository { get; }

        /// <summary>
        /// Repository for managing PermissionAction
        /// </summary>
        IPermissionActionRepository PermissionActionRepository { get; }

        // Future repositories can be added here:
        // IBookRepository Books { get; }
        // IAuthorRepository Authors { get; }
        // IPublisherRepository Publishers { get; }
        // ICategoryRepository Categories { get; }

        #endregion

        #region Save Changes

        /// <summary>
        /// Saves all changes made in the current unit of work to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Saves all changes made in the current DbContext to the database asynchronously.
        /// </summary>
        Task SaveChangesAsync();


        #endregion

        #region Transaction Handling

        /// <summary>
        /// Executes a synchronous action within a database transaction.
        /// Commits if successful, rolls back if an exception occurs.
        /// </summary>
        /// <param name="action">Synchronous action to execute inside the transaction.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task ExecuteInTransactionAsync(Action action, CancellationToken cancellationToken);

        /// <summary>
        /// Executes an asynchronous action within a database transaction.
        /// Commits if successful, rolls back if an exception occurs.
        /// </summary>
        /// <param name="action">Asynchronous action to execute inside the transaction.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken);

        #endregion
    }
}
