using BaseProject.Application.Common.Exceptions;
using BaseProject.Domain.Constants;
using BaseProject.Domain.Enums;
using BaseProject.Shared.DTOs.Common;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BaseProject.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async context =>
                {
                    context.Response.ContentType = "application/json";

                    var errorId = Guid.NewGuid().ToString();
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        string errorMessage;
                        string errorCode;

                        if (contextFeature.Error is FriendlyException userFriendlyException)
                        {
                            // Handle user-friendly exceptions based on error codes
                            switch (userFriendlyException.ErrorCode)
                            {
                                case ErrorCode.NotFound:
                                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.NOT_FOUND.ToString();
                                    break;
                                case ErrorCode.Conflict:
                                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.CONFLICT.ToString();
                                    break;
                                case ErrorCode.BadRequest:
                                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.BAD_REQUEST.ToString();
                                    break;
                                case ErrorCode.Unauthorized:
                                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.UNAUTHORIZED.ToString();
                                    break;
                                case ErrorCode.Forbidden:
                                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.FORBIDDEN.ToString();
                                    break;
                                case ErrorCode.Unauthenticated:
                                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.UNAUTHENTICATED.ToString();
                                    break;
                                case ErrorCode.Timeout:
                                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.TIMEOUT.ToString();
                                    break;
                                case ErrorCode.ServiceUnavailable:
                                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.SERVICE_UNAVAILABLE.ToString();
                                    break;
                                case ErrorCode.TooManyRequests:
                                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.TOO_MANY_REQUESTS.ToString();
                                    break;
                                case ErrorCode.InvalidOperation:
                                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.INVALID_OPERATION.ToString();
                                    break;
                                case ErrorCode.UnsupportedMediaType:
                                    context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.UNSUPPORTED_MEDIA_TYPE.ToString();
                                    break;
                                case ErrorCode.GatewayTimeout:
                                    context.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.GATEWAY_TIMEOUT.ToString();
                                    break;
                                case ErrorCode.MethodNotAllowed:
                                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                                    errorMessage = userFriendlyException.UserFriendlyMessage;
                                    errorCode = ErrorRespondCode.METHOD_NOT_ALLOWED.ToString();
                                    break;
                                default:
                                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    errorMessage = "An unexpected error occurred.";
                                    errorCode = ErrorRespondCode.GENERAL_ERROR.ToString();
                                    break;
                            }
                        }
                        else
                        {
                            // For non-user-friendly exceptions
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            errorMessage = "An error has occurred.";
                            errorCode = ErrorRespondCode.GENERAL_ERROR.ToString();
                        }

                        // Construct a user-friendly response
                        var errorResponse = ResponseDto<string>.Fail(errorMessage);

                        // Return error response to the client
                        await context.Response.WriteAsJsonAsync(errorResponse);

                        // Log the error for internal purposes
                        logger.LogError("ErrorId: {ErrorId}, Exception: {Exception}", errorId, contextFeature.Error);
                    }
                }
            });
        }
    }
}
