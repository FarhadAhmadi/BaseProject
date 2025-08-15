using BaseProject.API.Extensions;
using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxResponseLength = 1000; // truncate logs

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

            // Capture original response stream
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            bool success = true;

            try
            {
                // --- Log request ---
                await LogRequestAsync(context, traceId, correlationId);

                // Execute next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                success = false;
                //Log.Error(ex, "Unhandled exception | TraceId: {TraceId} | CorrelationId: {CorrelationId}", traceId, correlationId);
                throw; // let ExceptionHandlingMiddleware handle it
            }
            finally
            {
                stopwatch.Stop();

                if (success)
                {
                    // --- Log response only if request was successful ---
                    await LogResponseAsync(context, traceId, correlationId, stopwatch.ElapsedMilliseconds, responseBody, originalBodyStream);
                }
                else
                {
                    // On exception, just restore original stream so ExceptionHandlingMiddleware can write
                    context.Response.Body = originalBodyStream;
                }
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
            // Read response from memory stream
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            responseText = responseText.MaskSensitiveData();

            Log.Information("Outgoing response: {StatusCode} for {Method} {Path} in {Elapsed}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Response: {Response}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                elapsedMs,
                traceId,
                correlationId,
                responseText.Length > _maxResponseLength ? responseText[.._maxResponseLength] + "..." : responseText);

            // Write response back to original stream
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}
