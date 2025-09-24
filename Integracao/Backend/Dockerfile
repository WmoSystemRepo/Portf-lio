FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY PARS-Inhouse-Systems-API/*.csproj ./PARS-Inhouse-Systems-API/
COPY PARS.Inhouse.Systems.Application/*.csproj ./PARS.Inhouse.Systems.Application/
COPY PARS.Inhouse.Systems.Domain/*.csproj ./PARS.Inhouse.Systems.Domain/
COPY PARS.Inhouse.Systems.Infrastructure/*.csproj ./PARS.Inhouse.Systems.Infrastructure/
COPY PARS.Inhouse.Systems.Shared/*.csproj ./PARS.Inhouse.Systems.Shared/
COPY ["PARS-Inhouse-Systems-API/appsettings.json", "PARS-Inhouse-Systems-API/appsettings.Production.json", "PARS-Inhouse-Systems-API/appsettings.Staging.json", "./"]

RUN dotnet restore

COPY . .

WORKDIR /app/PARS-Inhouse-Systems-API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 7024
ENV ASPNETCORE_URLS=http://+:7024

ENTRYPOINT ["dotnet", "PARS-Inhouse-Systems-API.dll"]
