using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using ServerContainerManager.API.ErrorHandling;
using ServerContainerManager.API.Json;
using ServerContainerManager.API.Policies;
using ServerContainerManager.Application;
using ServerContainerManager.Application.Entities.Extensions;
using ServerContainerManager.Application.Entities;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .ReadFrom
        .Configuration(
            new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build())
    .CreateBootstrapLogger();

try
{
    Log.Information("Bootstrapping");
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    // Serilog Setup
    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services);
    });

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(AuthPolicies.AuthenticatedUserPolicy.Name, AuthPolicies.AuthenticatedUserPolicy.Policy)
        .AddPolicy(AuthPolicies.ConfirmedUserPolicy.Name, AuthPolicies.ConfirmedUserPolicy.Policy)
        .AddPolicy(AuthPolicies.UnconfirmedUserPolicy.Name, AuthPolicies.UnconfirmedUserPolicy.Policy)
        .AddPolicy(AuthPolicies.ConfirmedAdminPolicy.Name, AuthPolicies.ConfirmedAdminPolicy.Policy)
        .SetDefaultPolicy(AuthPolicies.ConfirmedUserPolicy.Policy);

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.SerializerOptions.Converters.Add(new JsonUtcDateTimeConverter());
    });

    builder.Services.RegisterApplicationLayerServices(
        appDbOptionsBuilder: options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("AppDb"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 3;

        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+!?";
        options.User.RequireUniqueEmail = false;
    });

    builder.Services.Configure<SecurityStampValidatorOptions>(options =>
    {
        options.ValidationInterval = TimeSpan.Zero; // Always validate the user to ensure correct forced logout and deletion
    });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

        options.SlidingExpiration = true;
    });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

#if DEBUG
    // Only needed in development because Angular dev server is on a different origin
    // In production the backend will serve its own files from wwwroot folder (same origin, no CORS needed)
    builder.Services.AddCors(options =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>()!;

        options.AddPolicy("FrontendPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
#endif

    Log.Information("Building");
    var app = builder.Build();
    Log.Information("Built");

    var isHttpsConfigured = builder.Configuration.GetSection("Kestrel:Endpoints:Https").Exists()
        || (Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Contains("https://", StringComparison.OrdinalIgnoreCase) ?? false);

#if DEBUG
    // Only needed in development because Angular dev server is on a different origin
    // In production the backend will serve its own files from wwwroot folder (same origin, no CORS needed)
    app.UseCors("FrontendPolicy");

    // Configure the HTTP request pipeline.
    app.MapOpenApi();
    app.MapScalarApiReference();
#endif

    app.MapDefaultEndpoints();
    if (isHttpsConfigured)
        app.UseHttpsRedirection();

    // Use wwwroot files for production environment
    // NOTE: On development environment there will be no wwwroot folder but this won't throw
    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseAuthorization();
    app.MapControllers();

    // Map fallback to index.html to let Angular router handle any path that isn't an API route (/api/*).
    app.MapFallbackToFile("index.html");

    app.UseExceptionHandler();

    await app.InitializeDatabaseAsync();
    await app.CreateAdminUserIfNotExists();

    Log.Information("Running");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Shutting down");
    await Log.CloseAndFlushAsync();
}
