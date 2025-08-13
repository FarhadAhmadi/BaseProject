using BaseProject.Application.DTOs.Common;
using BaseProject.Domain.Constants;
using BaseProject.Shared.DTOs.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BaseProject.API.Validations
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                List<ErrorDto> errors = [];
                foreach (var error in result.Errors)
                {
                    var validationError = new ErrorDto($"{ApplicationConstants.Name}.{ErrorRespondCode.BAD_REQUEST}", error.ErrorMessage);
                    validationError.AddErrorProperty(new ErrorProperty(error.PropertyName, error.AttemptedValue == null ? "null" : error.AttemptedValue?.ToString() ?? "null"));
                    errors.Add(validationError);

                    var errorResponse = new ErrorResponse(errors);
                    actionContext.ModelState.TryAddModelException(ApplicationConstants.FluentValidationErrorKey, new ValidationException(errorResponse.Errors.Select(x => x.Message).FirstOrDefault()));
                }
            }
            return result;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
