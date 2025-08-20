using BaseProject.API.Behaviors;
using BaseProject.Application.Behaviours;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Mappings;
using BaseProject.Application.Services;
using BaseProject.Domain.Configurations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BaseProject.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, AppSettings appsettings)
    {

        // Register MediatR and scan assemblies for handlers
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddValidatorsFromAssemblyContaining<Features.Auth.Commands.SignIn.SignInCommandHandler.SignInCommandValidator>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRRetryBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRLoggingAndExceptionBehavior<,>));


        services.AddAutoMapper(typeof(MapProfile).Assembly);

        services.AddTransient<IUserContext, UserContext>();
        //services.AddTransient<IBookService, BookService>();
        //services.AddTransient<IAuthorService, AuthorService>();
        //services.AddTransient<IPublisherService, PublisherService>();
        //services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IMediaService, MediaService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAuthIdentityService, AuthIdentityService>();

        if (appsettings.FileStorageSettings.LocalStorage)
        {
            services.AddSingleton<IFileService, LocalStorageService>();
        }
        else
        {
            services.AddSingleton<IFileService, CloudinaryStorageService>();
        }

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICurrentTime, CurrentTime>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
