using BaseProject.API.Extensions;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Shared.DTOs.Common;
using System.Diagnostics;
using System.Text.Json;

namespace BaseProject.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly IAppLogger _logger;

        private static readonly string[] SensitiveKeys = { "Authorization", "Cookie", "Set-Cookie", "password", "token" };

        public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment env, IAppLogger logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string requestBody = "[Unavailable]";
            try
            {
                if (_env.IsDevelopment() && !context.Request.Body.CanSeek)
                    context.Request.EnableBuffering();

                requestBody = _env.IsDevelopment()
                    ? await context.ReadRequestBodyAsync(SensitiveKeys)
                    : "[Hidden in production]";
            }
            catch (Exception readEx)
            {
                _logger.Warning($"Failed to read request body: {readEx.Message}", Activity.Current?.Id ?? "N/A");
            }

            int statusCode = StatusCodes.Status500InternalServerError;
            ResponseDto<object> response = ResponseDto<object>.FailResponse("An unexpected error occurred.");

            string traceId = Activity.Current?.Id ?? "N/A";

            if (exception is FriendlyException friendlyEx)
            {
                statusCode = (int)friendlyEx.ErrorCode;
                response = ResponseDto<object>.FailResponse(friendlyEx.Message);

                _logger.Warning($"Handled FriendlyException | Body: {requestBody} | TraceId: {traceId}");
            }
            else
            {
                _logger.Error(exception, $"Unhandled exception | Body: {requestBody} | TraceId: {traceId}");
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                try
                {
                    string json = _env.IsDevelopment()
                        ? JsonSerializer.Serialize(new
                        {
                            response.Success,
                            response.Message,
                            Exception = exception.Message,
                            ExceptionType = exception.GetType().FullName,
                            StackTrace = exception.StackTrace,
                            InnerException = exception.InnerException?.Message,
                            RequestBody = requestBody,
                            TraceId = traceId,
                            CorrelationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                        }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                        : JsonSerializer.Serialize(new
                        {
                            response.Success,
                            response.Message,
                            TraceId = traceId,
                            CorrelationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                        }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    await context.Response.WriteAsync(json);
                }
                catch (Exception writeEx)
                {
                    _logger.Error(writeEx, $"Failed to write exception response | TraceId: {traceId}");
                }
            }
            else
            {
                _logger.Warning("Response already started. Cannot write exception body | TraceId: {TraceId}", traceId);
            }
        }
    }
}
