using Microsoft.Extensions.DependencyInjection;

namespace Tasky.Application;

public static class DependencyInjection
{
    /// <summary>Registra los servicios de la capa de aplicación (casos de uso, validadores...).</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
