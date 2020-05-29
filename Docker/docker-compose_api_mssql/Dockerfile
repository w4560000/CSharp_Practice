FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY *.sln .
COPY docker-compose_api_mssql/*.csproj ./docker-compose_api_mssql/
RUN dotnet restore

COPY docker-compose_api_mssql/. ./docker-compose_api_mssql/
WORKDIR /app/docker-compose_api_mssql
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
EXPOSE 5000
WORKDIR /app
COPY --from=build /app/docker-compose_api_mssql/out ./
ENTRYPOINT ["dotnet", "docker-compose_api_mssql.dll"]