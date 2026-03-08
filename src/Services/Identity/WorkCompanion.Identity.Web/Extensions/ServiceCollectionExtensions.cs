using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using WorkCompanion.Common.Configuration.Configurations;
using WorkCompanion.Common.Configuration.Exceptions;
using WorkCompanion.Identity.Web.Models;
using WorkCompanion.Identity.Web.Validators;

namespace WorkCompanion.Identity.Web.Extensions;

/// <summary>
/// Extension methods for configuring web application services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures cookie policy options for the application
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        return services;
    }

    /// <summary>
    /// Configures the application authentication cookie settings
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services)
    {
        services.Configure<CookieAuthenticationOptions>(
            IdentityConstants.ApplicationScheme, options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
        });

        return services;
    }

    /// <summary>
    /// Configures Cross-Origin Resource Sharing (CORS) policy from configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection for method chaining</returns>
    /// <exception cref="ConfigurationException">Thrown when required CORS configuration is missing</exception>
    public static IServiceCollection ConfigureCors(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var corsConfiguration =
            configuration.GetSection("Cors").Get<CorsConfiguration>()
            ?? throw new ConfigurationException("Cors");

        var origins = corsConfiguration.Origins
            ?? throw new ConfigurationException(nameof(corsConfiguration.Origins));

        // Validate origins format
        foreach (var origin in origins)
        {
            if (!Uri.TryCreate(origin, UriKind.Absolute, out _))
            {
                throw new ConfigurationException($"Invalid CORS origin format: {origin}");
            }
        }

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy
                    .WithOrigins(origins)
                    .AllowCredentials()
                    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .WithHeaders("Content-Type", "Authorization", "Accept", "X-Requested-With")
                    .WithExposedHeaders("Content-Disposition", "X-Pagination")
            )
        );

        return services;
    }

    /// <summary>
    /// Configures FluentValidation for automatic model validation
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection ConfigureFluentValidation(this IServiceCollection services)
    {
        // Add FluentValidation auto-validation integration with ASP.NET Core MVC
        services.AddFluentValidationAutoValidation();

        // Register all validators from the Validators namespace
        services.AddScoped<IValidator<LoginInputModel>, LoginInputModelValidator>();
        services.AddScoped<IValidator<RegisterInputModel>, RegisterInputModelValidator>();
        services.AddScoped<IValidator<AuthorizeViewModel>, AuthorizeViewModelValidator>();
        services.AddScoped<IValidator<LoginViewModel>, LoginViewModelValidator>();
        services.AddScoped<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
        services.AddScoped<IValidator<LogoutViewModel>, LogoutViewModelValidator>();

        return services;
    }
}

