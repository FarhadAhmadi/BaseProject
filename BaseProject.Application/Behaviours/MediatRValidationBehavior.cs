using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Validation;
using BaseProject.Domain.Enums;
using FluentValidation;
using MediatR;

namespace BaseProject.Application.Behaviours
{
    /// <summary>
    /// Pipeline behavior to handle FluentValidation for requests.
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public class MediatRValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IBusinessValidator<TRequest>> _validators;

        public MediatRValidationBehavior(IEnumerable<IBusinessValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(
               TRequest request,
               RequestHandlerDelegate<TResponse> next,
               CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(request);
                if (!result.IsValid)
                {
                    throw new FriendlyException(
                        ApiErrorCode.BadRequest,
                        string.Join(";", result.Errors)
                    );
                }
            }

            return await next(cancellationToken);
        }
    }
}