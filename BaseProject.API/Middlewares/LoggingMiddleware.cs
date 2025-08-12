using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace BaseProject.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log request
            Log.Information("Incoming request: {Method} {Path} from {IpAddress}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress?.ToString());

            // Optionally log body (careful with large requests)
            if (context.Request.ContentLength > 0 &&
                context.Request.ContentType != null &&
                context.Request.ContentType.Contains("application/json"))
            {
                context.Request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                var requestBody = Encoding.UTF8.GetString(buffer);
                context.Request.Body.Position = 0;
                Log.Debug("Request body: {Body}", requestBody);
            }

            await _next(context);
            stopwatch.Stop();

            // Log response
            Log.Information("Outgoing response: {StatusCode} for {Method} {Path} in {Elapsed}ms",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
