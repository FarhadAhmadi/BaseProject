using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using CorrelationId.Abstractions;
using Serilog;

namespace BaseProject.API.Middlewares
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;

        public PerformanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var traceId = context.TraceIdentifier;
            var correlationContextAccessor = context.RequestServices.GetService<ICorrelationContextAccessor>();
            var correlationId = correlationContextAccessor?.CorrelationContext?.CorrelationId ?? traceId;

            await _next(context);

            stopwatch.Stop();

            Log.Information(
                "Request completed | Method: {Method} | Path: {Path} | StatusCode: {StatusCode} | TimeTaken: {Elapsed}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                traceId,
                correlationId
            );
        }
    }
}
