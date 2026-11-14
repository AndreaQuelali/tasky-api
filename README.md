# tasky-api

API de **TASKY**, una plataforma SaaS de gestión de proyectos y tareas. El frontend vive en el repositorio `tasky-web`.

## Stack

- .NET 10 (LTS) y ASP.NET Core Web API
- EF Core + PostgreSQL
- ASP.NET Core Identity + JWT
- SignalR para notificaciones en tiempo real
- Serilog, FluentValidation
- xUnit + Testcontainers
- Docker Compose para el entorno local

## Flujo de trabajo

- `main`: solo recibe merges desde `develop` al publicar.
- `develop`: rama de integración; las PRs apuntan aquí.
- `feature/*`: una rama por funcionalidad, creada desde `develop`.
- Commits con [Conventional Commits](https://www.conventionalcommits.org/) en inglés.

## Estructura

```
src/
  Tasky.Domain          Entidades y reglas de negocio (sin dependencias)
  Tasky.Application     Casos de uso (depende de Domain)
  Tasky.Infrastructure  EF Core + PostgreSQL (depende de Application)
  Tasky.Api             ASP.NET Core Web API (punto de entrada)
tests/
  Tasky.Api.Tests       Tests de integración (xUnit)
```

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (para PostgreSQL)

## Cómo ejecutarla

```bash
# 1. Base de datos
docker compose up -d db

# 2. API en http://localhost:5080
dotnet run --project src/Tasky.Api
```

- `GET /health/live`: la API responde.
- `GET /health`: además comprueba la conexión a PostgreSQL.
- `GET /openapi/v1.json`: contrato OpenAPI (solo en Development).

Para levantar todo en contenedores: `docker compose up --build`.

## Tests

```bash
dotnet test
```

## Configuración

Las opciones viven en `src/Tasky.Api/appsettings.json` y se sobrescriben con variables de entorno
(por ejemplo `ConnectionStrings__Postgres` o `Cors__AllowedOrigins__0`). El frontend (`tasky-web`)
corre en `http://localhost:5173`, que ya está permitido por CORS.
