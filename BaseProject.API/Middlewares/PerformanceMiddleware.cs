using System.Diagnostics;

namespace BaseProject.API.Middlewares
{
    public class PerformanceMiddleware
    {
        private readonly ILogger<PerformanceMiddleware> _logger;
        private readonly Stopwatch _stopwatch;
        private readonly RequestDelegate _next;

        public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
        {
            _next = next;
            _logger =   logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _stopwatch.Restart();
            await _next(context);
            _stopwatch.Stop();

            _logger.LogInformation("Time taken: {timeTaken}", _stopwatch.Elapsed.ToString(@"m\:ss\.fff"));
        }
    }
}
