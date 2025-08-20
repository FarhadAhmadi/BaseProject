using BaseProject.Application.Common.Interfaces;
using CorrelationId.Abstractions;
using System.Diagnostics;

namespace BaseProject.API.Middlewares
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger _logger;
        private readonly ICorrelationContextAccessor _correlationContext;

        public PerformanceMiddleware(RequestDelegate next, IAppLogger logger, ICorrelationContextAccessor correlationContext)
        {
            _next = next;
            _logger = logger;
            _correlationContext = correlationContext;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            _logger.Info(
                "Request completed | Method: {Method} | Path: {Path} | StatusCode: {StatusCode} | TimeTaken: {Elapsed}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                Activity.Current?.Id ?? "N/A",
                _correlationContext.CorrelationContext?.CorrelationId ?? "N/A"
            );
        }
    }
}
