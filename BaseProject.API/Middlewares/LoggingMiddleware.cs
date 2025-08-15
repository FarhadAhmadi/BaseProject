using BaseProject.API.Extensions;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BaseProject.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly int _maxResponseLength = 1000;

        public LoggingMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
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

            Log.Debug("Incoming request: {Method} {Path} from {IpAddress} | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress?.ToString(),
                Activity.Current?.Id,
                context.Request.Headers["X-Correlation-ID"].FirstOrDefault(),
                requestBody.Length > _maxResponseLength ? requestBody[.._maxResponseLength] + "..." : requestBody
            );
        }

        private async Task LogResponseAsync(HttpContext context, long elapsedMs, MemoryStream responseBody, Stream originalBodyStream)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            responseText = _env.IsProduction() ? "[Hidden in production]" : responseText.MaskSensitiveData();

            Log.Information("Outgoing response: {StatusCode} for {Method} {Path} in {Elapsed}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Response: {Response}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                elapsedMs,
                Activity.Current?.Id,
                context.Request.Headers["X-Correlation-ID"].FirstOrDefault(),
                responseText.Length > _maxResponseLength ? responseText[.._maxResponseLength] + "..." : responseText
            );

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}
