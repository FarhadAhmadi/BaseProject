using BaseProject.Application.Common.Interfaces;
using BaseProject.Shared.Extensions;
using CorrelationId.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BaseProject.API.Behaviors
{
    /// <summary>
    /// Pipeline behavior to log requests and responses for MediatR commands/queries.
    /// Also logs exceptions if they occur.
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    public class MediatRLoggingAndExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _env;
        private readonly int _maxPayloadLength = 1000;
        private readonly ICorrelationContextAccessor _correlationContext;
        private readonly IAppLogger _logger;

        public MediatRLoggingAndExceptionBehavior(IHttpContextAccessor httpContextAccessor, IHostEnvironment env, ICorrelationContextAccessor correlationContext, IAppLogger logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _correlationContext = correlationContext;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var correlationId = _correlationContext.CorrelationContext?.CorrelationId;
            var traceId = Activity.Current?.Id;

            // Serialize request payload
            string requestPayload;
            try
            {
                requestPayload = JsonSerializer.Serialize(request);
                if (_env.IsProduction())
                    requestPayload = "[Hidden in production]";
                else
                    requestPayload = requestPayload.MaskSensitiveData();

                if (requestPayload.Length > _maxPayloadLength)
                    requestPayload = requestPayload[.._maxPayloadLength] + "...";
            }
            catch
            {
                requestPayload = "[Unavailable]";
            }

            var stopwatch = Stopwatch.StartNew();
            _logger.Debug(
                "Handling Command/Query: {RequestName} | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Payload: {Payload}",
                requestName, traceId, correlationId, requestPayload
            );

            try
            {
                var response = await next();

                stopwatch.Stop();

                string responsePayload;
                try
                {
                    responsePayload = JsonSerializer.Serialize(response);
                    if (_env.IsProduction())
                        responsePayload = "[Hidden in production]";
                    else
                        responsePayload = responsePayload.MaskSensitiveData();

                    if (responsePayload.Length > _maxPayloadLength)
                        responsePayload = responsePayload[.._maxPayloadLength] + "...";
                }
                catch
                {
                    responsePayload = "[Unavailable]";
                }

                _logger.Info(
                    "Handled Command/Query: {RequestName} in {ElapsedMs}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Response: {Response}",
                    requestName, stopwatch.ElapsedMilliseconds, traceId, correlationId, responsePayload
                );

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(
                    ex,
                    "Error handling Command/Query: {RequestName} after {ElapsedMs}ms | TraceId: {TraceId} | CorrelationId: {CorrelationId} | Payload: {Payload}",
                    requestName, stopwatch.ElapsedMilliseconds, traceId, correlationId, requestPayload
                );
                throw;
            }
        }
    }
}
