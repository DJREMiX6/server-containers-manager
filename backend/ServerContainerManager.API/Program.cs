using Serilog;
using ServerContainerManager.Application.Services;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using ServerContainerManager.Application;

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

    builder.Services.RegisterApplicationLayerServices();

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
