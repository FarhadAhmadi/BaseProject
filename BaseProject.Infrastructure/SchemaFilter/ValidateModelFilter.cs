using BaseProject.Application.DTOs.Common;
using BaseProject.Domain.Constants;
using BaseProject.Shared.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseProject.Infrastructure.SchemaFilter
{
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation needed for this filter.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Check for FluentValidation errors first.
                if (context.ModelState.TryGetValue(ApplicationConstants.FluentValidationErrorKey, out var fluentErrorEntry) && fluentErrorEntry.Errors.Any())
                {
                    HandleFluentValidationErrors(context, fluentErrorEntry);
                    return;
                }

                // Handle general model validation errors.
                HandleModelValidationErrors(context);
            }
        }

        private void HandleFluentValidationErrors(ActionExecutingContext context, ModelStateEntry fluentErrorEntry)
        {
            if (fluentErrorEntry.Errors.First().Exception is ValidationException exception)
            {
                context.Result = new BadRequestObjectResult(exception.Value);
            }
        }

        private void HandleModelValidationErrors(ActionExecutingContext context)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .SelectMany(ms => ms.Value.Errors.Select(error => new
                {
                    Key = ms.Key,
                    ErrorMessage = error.ErrorMessage,
                    AttemptedValue = ms.Value.AttemptedValue
                }))
                .Select(errorDetail => new ErrorDto(
                    $"{ApplicationConstants.Name}.{ErrorRespondCode.BAD_REQUEST}",
                    errorDetail.ErrorMessage)
                {
                    Property = errorDetail.ErrorMessage ?? "null"
                })
                .ToList();

            context.Result = new BadRequestObjectResult(new ErrorResponse { Errors = errors });
        }
    }
}
