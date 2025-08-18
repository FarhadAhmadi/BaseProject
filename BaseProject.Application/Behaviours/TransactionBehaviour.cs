using BaseProject.Domain.Interfaces;
using MediatR;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehaviour(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response = default;

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            response = await next();
        }, cancellationToken);

        return response;
    }
}
