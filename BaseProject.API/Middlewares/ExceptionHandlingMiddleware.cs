using BaseProject.API.Extensions;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Shared.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
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
                Log.Warning(readEx, "Failed to read request body");
            }

            int statusCode = StatusCodes.Status500InternalServerError;
            var response = ResponseDto<object>.FailResponse("An unexpected error occurred.");

            if (exception is FriendlyException friendlyEx)
            {
                statusCode = (int)friendlyEx.ErrorCode;
                response = ResponseDto<object>.FailResponse(friendlyEx.Message);

                if (_env.IsDevelopment())
                {
                    Log.Warning(exception, "Handled FriendlyException | Body: {Body}", requestBody);
                }
                else
                {
                    Log.Warning("Handled FriendlyException | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                        Activity.Current?.Id,
                        context.Request.Headers["X-Correlation-ID"].FirstOrDefault());
                }
            }
            else
            {
                if (_env.IsDevelopment())
                {
                    Log.Error(exception, "Unhandled exception | Body: {Body}", requestBody);
                }
                else
                {
                    Log.Error(exception, "Unhandled exception | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                        Activity.Current?.Id,
                        context.Request.Headers["X-Correlation-ID"].FirstOrDefault());
                }
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
                        TraceId = Activity.Current?.Id,
                        CorrelationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                    }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    await context.Response.WriteAsync(json);
                }
                catch (Exception writeEx)
                {
                    Log.Error(writeEx, "Failed to write exception response");
                }
            }
            else
            {
                Log.Warning("Response already started. Cannot write exception body");
            }
        }
    }
}
