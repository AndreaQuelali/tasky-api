using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Infrastructure.Persistence;

namespace Tasky.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Registra EF Core con PostgreSQL y el health check de la base de datos.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskyDbContext>(options => options.UseNpgsql(connectionString));

        services.AddHealthChecks()
            .AddDbContextCheck<TaskyDbContext>(name: "postgres", tags: ["ready"]);

        return services;
    }
}
