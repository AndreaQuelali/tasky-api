using Microsoft.EntityFrameworkCore;

namespace Tasky.Infrastructure.Persistence;

public sealed class TaskyDbContext(DbContextOptions<TaskyDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Carga todas las IEntityTypeConfiguration<T> de este ensamblado.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskyDbContext).Assembly);
    }
}
