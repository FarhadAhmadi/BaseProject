using MediatR;
using Microsoft.Extensions.Logging;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {RequestName} with data: {@Request}", typeof(TRequest).Name, request);

        var response = await next();

        _logger.LogInformation("Handled {RequestName} with response: {@Response}", typeof(TRequest).Name, response);

        return response;
    }
}
