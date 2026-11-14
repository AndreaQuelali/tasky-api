using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Tasky.Application;
using Tasky.Infrastructure;

// Logger mínimo para capturar errores de arranque antes de cargar la configuración.
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    var connectionString = builder.Configuration.GetConnectionString("Postgres")
        ?? throw new InvalidOperationException("Falta la cadena de conexión 'ConnectionStrings:Postgres'.");

    builder.Services
        .AddApplication()
        .AddInfrastructure(connectionString);

    builder.Services.AddProblemDetails();
    builder.Services.AddOpenApi();

    // El frontend (tasky-web) corre en otro origen; AllowCredentials será necesario para la cookie del refresh token.
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    builder.Services.AddCors(options =>
        options.AddPolicy("Frontend", policy => policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

    var app = builder.Build();

    app.UseExceptionHandler();
    app.UseStatusCodePages();
    app.UseSerilogRequestLogging();
    app.UseCors("Frontend");

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    // /health/live: el proceso responde. /health: además comprueba la base de datos.
    app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "La aplicación se detuvo de forma inesperada");
}
finally
{
    Log.CloseAndFlush();
}

// Necesario para que los tests de integración (WebApplicationFactory) puedan referenciar Program.
public partial class Program;
