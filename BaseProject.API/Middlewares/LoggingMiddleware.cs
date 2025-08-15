using BaseProject.API.Extensions;
using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseProject.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxResponseLength = 1000; // truncate responses

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var traceId = context.TraceIdentifier;

            var correlationContextAccessor = context.RequestServices.GetService<ICorrelationContextAccessor>();
            var correlationId = correlationContextAccessor?.CorrelationContext?.CorrelationId ?? traceId;

            try
            {
                // --- Log request ---
                await LogRequestAsync(context, traceId, correlationId);

                // --- Capture response ---
                var originalBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context);

                stopwatch.Stop();

                // --- Log response ---
                await LogResponseAsync(context, traceId, correlationId, stopwatch.ElapsedMilliseconds, responseBody, originalBodyStream);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Log.Error(ex, "Unhandled exception | TraceId: {TraceId} | CorrelationId: {CorrelationId}", traceId, correlationId);
                throw;
            }
        }

        private async Task LogRequestAsync(HttpContext context, string traceId, string correlationId)
        {
            string requestBody = null;
            if (context.Request.ContentLength > 0 &&
                context.Request.ContentType?.Contains("application/json") == true)
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                // Mask sensitive data like passwords
                requestBody = requestBody.MaskSensitiveData();

                Log.Debug("Incoming request: {Method} {Path} from {IpAddress} | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Body: {Body}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Connection.RemoteIpAddress?.ToString(),
                    traceId,
                    correlationId,
                    requestBody.Length > _maxResponseLength ? requestBody[.._maxResponseLength] + "..." : requestBody);
            }
            else
            {
                Log.Information("Incoming request: {Method} {Path} from {IpAddress} | TraceId: {TraceId} | CorrelationId: {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Connection.RemoteIpAddress?.ToString(),
                    traceId,
                    correlationId);
            }
        }

        private async Task LogResponseAsync(HttpContext context, string traceId, string correlationId, long elapsedMs, MemoryStream responseBody, Stream originalBodyStream)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Mask sensitive fields in response if needed
            responseText = responseText.MaskSensitiveData();

            Log.Information("Outgoing response: {StatusCode} for {Method} {Path} in {Elapsed}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Response: {Response}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                elapsedMs,
                traceId,
                correlationId,
                responseText.Length > _maxResponseLength ? responseText[.._maxResponseLength] + "..." : responseText);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
