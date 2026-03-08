using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using WorkCompanion.Common.Configuration.Exceptions;
using WorkCompanion.Identity.Application.Constants;
using WorkCompanion.Identity.Application.Dtos;
using WorkCompanion.Identity.Application.Services;
using WorkCompanion.Identity.Application.Validators;
using WorkCompanion.Identity.Domain.Entities;
using WorkCompanion.Identity.Infrastructure.Data;
using WorkCompanion.Identity.Infrastructure.Services;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace WorkCompanion.Identity.Infrastructure;

/// <summary>
/// Dependency injection extension methods for infrastructure services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures infrastructure services including database, identity, OpenIddict, and Quartz
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection for method chaining</returns>
    [RequiresUnreferencedCode(
        "Infrastructure configuration requires reflection for configuration binding and identity setup"
    )]
    public static IServiceCollection ConfigureInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureDbContext(configuration);
        services.ConfigureIdentity();
        services.ConfigureOpenIddict(configuration);
        services.ConfigureQuartz();
        services.ConfigureServices();

        return services;
    }

    /// <summary>
    /// Initializes the infrastructure by running pending database migrations
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for method chaining</returns>
    public static async Task<IApplicationBuilder> UseInfrastructureAsync(
        this IApplicationBuilder app
    )
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();

        return app;
    }

    [RequiresUnreferencedCode(
        "Configuration binding requires reflection"
    )]
    private static IServiceCollection ConfigureDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var databaseProvider =
            configuration.GetValue<string>("DatabaseProvider")
            ?? throw new ConfigurationException("DatabaseProvider");
        var connectionString =
            configuration.GetConnectionString("Application")
            ?? throw new ConfigurationException("ConnectionStrings:Application");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            switch (databaseProvider)
            {
                case "Sqlite":
                    options.UseSqlite(
                        connectionString,
                        x =>
                            x.MigrationsAssembly(
                                "WorkCompanion.Identity.Infrastructure.Migrations.Sqlite"
                            )
                    );
                    break;
                default:
                    throw new Exception($"Unsupported provider: {databaseProvider}.");
            }

            options.UseOpenIddict<
                ApplicationClient,
                ApplicationAuthorization,
                ApplicationScope,
                ApplicationToken,
                Guid
            >();
        });

        return services;
    }

    [RequiresUnreferencedCode(
        "AddIdentity requires reflection for identity types"
    )]
    private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                options.SignIn.RequireConfirmedEmail = false
            )
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    [RequiresUnreferencedCode(
        "Configuration binding and OpenIddict setup require reflection"
    )]
    private static IServiceCollection ConfigureOpenIddict(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options
                    .UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>()
                    .ReplaceDefaultEntities<
                        ApplicationClient,
                        ApplicationAuthorization,
                        ApplicationScope,
                        ApplicationToken,
                        Guid
                    >();

                options.UseQuartz();
            })
            .AddServer(options =>
            {
                options
                    .SetAuthorizationEndpointUris("authorize")
                    .SetEndSessionEndpointUris("logout")
                    .SetIntrospectionEndpointUris("introspection")
                    .SetTokenEndpointUris("token")
                    .SetUserInfoEndpointUris("userinfo");

                options.RegisterScopes(
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.Roles,
                    AppScopes.Services.Posts
                );

                options.AllowAuthorizationCodeFlow().AllowRefreshTokenFlow();

                options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

                options
                    .UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableEndSessionEndpointPassthrough()
                    .EnableStatusCodePagesIntegration()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

                var validIssuers =
                    configuration.GetSection("ValidIssuers").Get<string[]>()
                    ?? throw new ConfigurationException("ValidIssuers");

                options.Configure(x => x.TokenValidationParameters.ValidIssuers = validIssuers);
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        return services;
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddScoped<IValidator<ApplicationUserDto>, ApplicationUserDtoValidator>();

        return services;
    }

    private static IServiceCollection ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
