using BaseProject.API.Extensions;
using BaseProject.API.Middlewares;
using BaseProject.Domain.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppSettings>()
    ?? throw new Exception("AppSetting Not Exist");

// -------------------- Configure Logging --------------------
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// -------------------- Configure Services --------------------

builder.Services.AddControllers()
    .AddApplicationPart(typeof(BaseProject.API.Controllers.BaseController).Assembly);


// Extension classes
builder.Services.AddHealthChecks();

var app = builder.Build();

// -------------------- Configure Middleware Pipeline --------------------
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();
// Set up CORS
app.UseCors("DefaultCorsPolicy");
// Swagger UI setup for API documentation
app.UseSwagger(configuration);
// Health check configuration
app.ConfigureHealthCheck();
// Set Rate Limmiter
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Authentication and Authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

// -------------------- Endpoints --------------------
app.MapControllers();
app.MapGet("/", () => "Hello World!");

// -------------------- Run Application --------------------
app.Run();
