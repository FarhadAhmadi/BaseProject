using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace BaseProject.Application.Behaviours
{
    /// <summary>
    /// Pipeline behavior to handle Retry for requests.
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public class MediatRRetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly AsyncRetryPolicy _retryPolicy;

        public MediatRRetryBehavior()
        {
            _retryPolicy = Policy
                .Handle<SqlException>()
                .Or<DbUpdateException>(ex =>
                    ex.InnerException is SqlException)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt)
                );
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            return await _retryPolicy.ExecuteAsync(() => next());
        }
    }
}
