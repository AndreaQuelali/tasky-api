namespace Tasky.Domain.Common;

/// <summary>Base de todas las entidades. Usa GUID v7 (ordenable por fecha de creación).</summary>
public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.CreateVersion7();
}
