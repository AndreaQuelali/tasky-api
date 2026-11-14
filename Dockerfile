# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Primero solo los archivos de proyecto, para aprovechar la caché de restore.
COPY global.json Directory.Build.props Directory.Packages.props ./
COPY src/Tasky.Domain/Tasky.Domain.csproj src/Tasky.Domain/
COPY src/Tasky.Application/Tasky.Application.csproj src/Tasky.Application/
COPY src/Tasky.Infrastructure/Tasky.Infrastructure.csproj src/Tasky.Infrastructure/
COPY src/Tasky.Api/Tasky.Api.csproj src/Tasky.Api/
RUN dotnet restore src/Tasky.Api/Tasky.Api.csproj

COPY src/ src/
RUN dotnet publish src/Tasky.Api/Tasky.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "Tasky.Api.dll"]
