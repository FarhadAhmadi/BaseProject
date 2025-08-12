using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;
using System.Text.Json;

namespace BaseProject.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

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
            var traceId = Guid.NewGuid().ToString();
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Structured logging with Serilog
            Log.Error(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);

            var problemDetails = new
            {
                type = "https://httpstatuses.io/500",
                title = "An unexpected error occurred.",
                status = context.Response.StatusCode,
                detail = _env.IsDevelopment() ? exception.ToString() : "Internal Server Error",
                instance = context.Request.Path,
                traceId
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
        }
    }
}
