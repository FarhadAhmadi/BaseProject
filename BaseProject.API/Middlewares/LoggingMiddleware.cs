using BaseProject.Application.Common.Interfaces;
using BaseProject.Shared.Extensions;
using CorrelationId.Abstractions;
using System.Diagnostics;
using System.Text;

namespace BaseProject.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly IAppLogger _logger;
        private readonly int _maxResponseLength = 1000;
        private readonly ICorrelationContextAccessor _correlationContext;

        public LoggingMiddleware(RequestDelegate next, IHostEnvironment env, IAppLogger logger,
            ICorrelationContextAccessor correlationContext)
        {
            _next = next;
            _env = env;
            _logger = logger;
            _correlationContext = correlationContext;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await LogRequestAsync(context);
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                await LogResponseAsync(context, stopwatch.ElapsedMilliseconds, responseBody, originalBodyStream);
            }
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            string requestBody = "[Empty]";
            try
            {
                if (context.Request.ContentLength > 0 &&
                    context.Request.ContentType?.Contains("application/json") == true)
                {
                    context.Request.EnableBuffering();
                    using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    requestBody = _env.IsProduction() ? "[Hidden in production]" : requestBody.MaskSensitiveData();
                }
            }
            catch
            {
                requestBody = "[Unavailable]";
            }

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "N/A";
            var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "N/A";

            _logger.Debug(
                "Incoming request: {Method} {Path} | IP: {IpAddress} | UserAgent: {UserAgent} | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                ip,
                userAgent,
                Activity.Current?.Id ?? "N/A",
                _correlationContext.CorrelationContext?.CorrelationId ?? "N/A",
                requestBody.Length > _maxResponseLength ? requestBody[.._maxResponseLength] + "..." : requestBody
            );
        }

        private async Task LogResponseAsync(HttpContext context, long elapsedMs, MemoryStream responseBody, Stream originalBodyStream)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            responseText = _env.IsProduction() ? "[Hidden in production]" : responseText.MaskSensitiveData();

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "N/A";
            var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "N/A";

            _logger.Info(
                "Outgoing response: {StatusCode} for {Method} {Path} | IP: {IpAddress} | UserAgent: {UserAgent} | Elapsed: {Elapsed}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Response: {Response}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                ip,
                userAgent,
                elapsedMs,
                Activity.Current?.Id ?? "N/A",
                _correlationContext.CorrelationContext?.CorrelationId ?? "N/A",
                responseText.Length > _maxResponseLength ? responseText[.._maxResponseLength] + "..." : responseText
            );

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}
