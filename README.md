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
