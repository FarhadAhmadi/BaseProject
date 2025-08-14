using BaseProject.Domain.Configurations;
using BaseProject.Infrastructure.SchemaFilter;
using Microsoft.OpenApi.Models;

namespace BaseProject.API.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerOpenAPI(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = appSettings.ApplicationDetail.ApplicationName,
                    Version = "v1",
                    Description = appSettings.ApplicationDetail.Description,
                    Contact = new OpenApiContact
                    {
                        Email = "farhadahmadi1413@gmail.com",
                        Name = "Farhad Ahmadi",
                        Url = new Uri(appSettings.ApplicationDetail.ContactWebsite),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // Enable XML comments
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

                // Add JWT Authorization to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.SchemaFilter<EnumSchemaFilter>();
                options.DocumentFilter<HealthChecksFilter>();
                options.EnableAnnotations();
            });

            return services;
        }

        public static void UseSwagger(this IApplicationBuilder app, AppSettings appSettings)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";

                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new[]
                    {
                        new OpenApiServer { Url = "https://localhost:7240" }
                    };
                });
            });

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "BaseProject.api v1");
                setupAction.RoutePrefix = "swagger"; // URL: https://localhost:7240/swagger
            });
        }
    }
}
