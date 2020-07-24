FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY K8S_Sample/. ./K8S_Sample/
WORKDIR /app/K8S_Sample
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
EXPOSE 5000
WORKDIR /app
COPY --from=build /app/K8S_Sample/out ./
ENTRYPOINT ["dotnet", "K8S_Sample.dll"]