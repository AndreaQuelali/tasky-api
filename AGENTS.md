# AGENTS.md

Guía para agentes de código y colaboradores que trabajen en **tasky-api**, la API de TASKY (plataforma SaaS de gestión de proyectos y tareas). El frontend vive en el repositorio `tasky-web`.

## Comandos

```bash
dotnet build                                   # compila la solución (Tasky.slnx)
dotnet test                                    # ejecuta los tests
docker compose up -d db                        # PostgreSQL local
dotnet run --project src/Tasky.Api             # API en http://localhost:5080
docker compose up --build                      # API + base de datos en contenedores
```

Antes de dar un cambio por terminado, ejecuta `dotnet build` y `dotnet test`. Endpoints útiles: `/health/live`, `/health` y `/openapi/v1.json` (solo Development).

## Stack

.NET 10 (LTS) · ASP.NET Core Web API · EF Core + PostgreSQL (Npgsql) · Serilog · OpenAPI · xUnit.
Previsto: ASP.NET Core Identity + JWT (refresh token en cookie HttpOnly), SignalR, FluentValidation, Testcontainers.

## Arquitectura

Solución por capas en `src/`. Las dependencias van solo hacia adentro:

- `Tasky.Domain`: entidades y reglas de negocio. Sin dependencias de otros proyectos ni paquetes de infraestructura.
- `Tasky.Application`: casos de uso. Depende solo de Domain.
- `Tasky.Infrastructure`: EF Core, `TaskyDbContext` y acceso a servicios externos. Depende de Application.
- `Tasky.Api`: punto de entrada (Program.cs, endpoints, configuración). Depende de Application e Infrastructure.
- `tests/Tasky.Api.Tests`: tests de integración con `WebApplicationFactory<Program>`.

Cada capa registra sus servicios en un `DependencyInjection.cs` (`AddApplication`, `AddInfrastructure`). No registres servicios de una capa desde otra.

## Convenciones de código

- Namespaces con alcance de archivo (`namespace Tasky.Domain.Common;`). `Nullable` habilitado: no desactives las advertencias de nulabilidad.
- Las versiones de paquetes se fijan solo en `Directory.Packages.props` (gestión central). En los `.csproj` va `<PackageReference Include="..." />` sin versión.
- Entidades heredan de `Entity` (Id GUID v7). Las que pertenecen a una organización implementan `ITenantEntity` y se filtran por `OrganizationId` con un filtro global de EF Core. Nunca devuelvas datos de otra organización.
- Configuración de EF Core con `IEntityTypeConfiguration<T>` (se cargan solos desde el ensamblado de Infrastructure); sin atributos de EF en el dominio.
- Configuración por `appsettings.json` y variables de entorno (`ConnectionStrings__Postgres`, `Cors__AllowedOrigins__0`). No subas secretos al repositorio.
- El contrato OpenAPI es la fuente de verdad: el cliente de `tasky-web` se genera a partir de él, así que cuida los nombres y tipos de los endpoints y respuestas.
- Los errores se devuelven como ProblemDetails.
- Comentarios y mensajes visibles en español; nombres de código en inglés.

## Migraciones

Cuando existan entidades, las migraciones se crean con `dotnet ef migrations add <Nombre> --project src/Tasky.Infrastructure --startup-project src/Tasky.Api` y se versionan en el repositorio.

## Git y Pull Requests

- Ramas: `main` solo recibe merges desde `develop` al publicar. Cada funcionalidad va en `feature/<nombre>`, creada desde `develop`, y su PR apunta a `develop`.
- Commits con [Conventional Commits](https://www.conventionalcommits.org/) **en inglés** (`feat(auth): add login endpoint`). Sin trailers de coautoría.
- Las PRs se redactan en inglés con las secciones Summary, Changes, How to test y Notes.
- Haz commit solo cuando se pida explícitamente.
