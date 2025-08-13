namespace BaseProject.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        //IBookRepository BookRepository { get; }
        //IAuthorRepository AuthorRepository { get; }
        //IPublisherRepository PublisherRepository { get; }
        //ICategoryRepository CategoryRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IMediaRepository MediaRepository { get; }
        IForgotPasswordRepository ForgotPasswordRepository { get; }
        Task SaveChangesAsync(CancellationToken token);
        Task ExecuteTransactionAsync(Action action, CancellationToken token);
        Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token);
    }
}
