using BaseProject.Domain.Entities;
using BaseProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace BaseProject.Infrastructure.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // ClaimsIdentity settings
                options.ClaimsIdentity = new ClaimsIdentityOptions
                {
                    UserIdClaimType = ClaimTypes.NameIdentifier,
                    UserNameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                    SecurityStampClaimType = "AspNet.Identity.SecurityStamp"
                };

                // User settings
                options.User = new UserOptions
                {
                    RequireUniqueEmail = true,
                    AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
                };

                // Password settings
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true,
                    RequiredLength = 6,
                    RequiredUniqueChars = 1
                };

                // Lockout settings
                options.Lockout = new LockoutOptions
                {
                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
                    MaxFailedAccessAttempts = 5,
                    AllowedForNewUsers = true
                };

                // SignIn settings
                options.SignIn = new SignInOptions
                {
                    RequireConfirmedAccount = false,
                    RequireConfirmedEmail = false,
                    RequireConfirmedPhoneNumber = false
                };

                // Token providers (default)
                options.Tokens = new TokenOptions
                {
                    AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider,
                    ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider,
                    ChangePhoneNumberTokenProvider = TokenOptions.DefaultPhoneProvider,
                    EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider,
                    PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider
                };
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "YourAppIdentityCookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Password hasher options
            services.Configure<PasswordHasherOptions>(options =>
            {
                options.IterationCount = 12000;
                options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
            });

            return services;
        }
    }
}
