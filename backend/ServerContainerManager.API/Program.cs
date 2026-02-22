using Serilog;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using ServerContainerManager.Application;
using ServerContainerManager.Application.Entities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ServerContainerManager.API.ErrorHandling;

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

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    builder.Services.RegisterApplicationLayerServices(
        appDbOptionsBuilder: options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("AppDb")));

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

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

        options.SlidingExpiration = true;
    });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.MapDefaultEndpoints();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

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
