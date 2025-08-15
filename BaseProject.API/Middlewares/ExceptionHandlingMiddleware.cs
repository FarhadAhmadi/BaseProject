using BaseProject.API.Extensions;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Shared.DTOs.Common;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseProject.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        private static readonly string[] SensitiveKeys = { "Authorization", "Cookie", "Set-Cookie", "password", "token" };

        public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = context.TraceIdentifier;
            var correlationContextAccessor = context.RequestServices.GetService<ICorrelationContextAccessor>();
            var correlationId = correlationContextAccessor?.CorrelationContext?.CorrelationId ?? traceId;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, traceId, correlationId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string traceId, string correlationId)
        {
            string body = "[Unavailable]";
            try
            {
                if (_env.IsDevelopment() && !context.Request.Body.CanSeek)
                    context.Request.EnableBuffering();

                body = _env.IsDevelopment()
                    ? await context.ReadRequestBodyAsync(SensitiveKeys)
                    : "[Hidden in production]";
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to read request body | TraceId: {TraceId}", traceId);
            }

            int statusCode = StatusCodes.Status500InternalServerError;
            var response = ResponseDto<object>.FailResponse("An unexpected error occurred.");

            if (exception is FriendlyException friendlyEx)
            {
                statusCode = (int)friendlyEx.ErrorCode;
                response = ResponseDto<object>.FailResponse(friendlyEx.Message);
                Log.Warning(exception, "Handled FriendlyException | TraceId: {TraceId}", traceId);
            }
            else
            {
                Log.Error(exception, "Unhandled exception | TraceId: {TraceId} | Body: {Body}", traceId, body);
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                try
                {
                    var json = JsonSerializer.Serialize(new
                    {
                        response.Success,
                        response.Message,
                        TraceId = traceId,
                        CorrelationId = correlationId
                    }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    await context.Response.WriteAsync(json);
                }
                catch (Exception writeEx)
                {
                    Log.Error(writeEx, "Failed to write exception response | TraceId: {TraceId}", traceId);
                }
            }
            else
            {
                Log.Warning("Response already started. Cannot write exception body | TraceId: {TraceId}", traceId);
            }
        }
    }
}
