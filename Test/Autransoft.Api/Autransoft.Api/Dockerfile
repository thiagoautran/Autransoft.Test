#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TAN.Core.5.0.Skokka.Api/TAN.Core.5.0.Skokka.Api.csproj", "TAN.Core.5.0.Skokka.Api/"]
COPY ["TAN.Core.5.0.Skokka.Infrastructure/TAN.Core.5.0.Skokka.Infrastructure.csproj", "TAN.Core.5.0.Skokka.Infrastructure/"]
COPY ["TAN.Core.5.0.Skokka.ApplicationCore/TAN.Core.5.0.Skokka.ApplicationCore.csproj", "TAN.Core.5.0.Skokka.ApplicationCore/"]
RUN dotnet restore "TAN.Core.5.0.Skokka.Api/TAN.Core.5.0.Skokka.Api.csproj"
COPY . .
WORKDIR "/src/TAN.Core.5.0.Skokka.Api"
RUN dotnet build "TAN.Core.5.0.Skokka.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TAN.Core.5.0.Skokka.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TAN.Core.5.0.Skokka.Api.dll"]
