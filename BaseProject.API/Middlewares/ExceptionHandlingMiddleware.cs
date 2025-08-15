using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using CorrelationId.Abstractions;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BaseProject.API.Extensions;

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
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var headers = context.Request.Headers.ToDictionary(
                h => h.Key,
                h => IsSensitiveKey(h.Key) ? "****" : string.Join(",", h.Value)
            );

            var query = context.Request.Query.ToDictionary(
                q => q.Key,
                q => IsSensitiveKey(q.Key) ? "****" : string.Join(",", q.Value)
            );

            // Optional: Read and mask request body safely
            string body = await ReadRequestBodyAsync(context);

            Log.Error(exception,
                "Unhandled exception | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Method: {Method} | Path: {Path} | Query: {Query} | Headers: {Headers} | Body: {Body}",
                traceId,
                correlationId,
                context.Request.Method,
                context.Request.Path,
                query,
                headers,
                body
            );

            var problemDetails = new
            {
                type = "https://httpstatuses.io/500",
                title = "An unexpected error occurred.",
                status = context.Response.StatusCode,
                detail = _env.IsDevelopment() ? exception.ToString() : "Internal Server Error",
                instance = context.Request.Path,
                traceId,
                correlationId
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(problemDetails, options);

            await context.Response.WriteAsync(result);
        }

        private static bool IsSensitiveKey(string key) =>
            SensitiveKeys.Contains(key, System.StringComparer.OrdinalIgnoreCase);

        private static async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true
                );

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                // Optional: Mask sensitive keys in JSON body
                if (!string.IsNullOrWhiteSpace(body) && body.TrimStart().StartsWith("{"))
                {
                    using var doc = JsonDocument.Parse(body);
                    var masked = doc.RootElement.MaskSensitiveJson();
                    return masked;
                }

                return body;
            }
            catch
            {
                return "[Unable to read body]";
            }
        }
    }
}
