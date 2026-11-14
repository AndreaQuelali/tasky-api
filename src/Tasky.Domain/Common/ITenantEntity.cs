namespace Tasky.Domain.Common;

/// <summary>
/// Marca las entidades que pertenecen a una organización (multi-tenancy).
/// Más adelante, EF Core filtrará por <see cref="OrganizationId"/> con un filtro global.
/// </summary>
public interface ITenantEntity
{
    Guid OrganizationId { get; }
}
