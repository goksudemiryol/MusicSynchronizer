# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MusicSynchronizer.WebApiHost/MusicSynchronizer.WebApiHost.csproj", "MusicSynchronizer.WebApiHost/"]
COPY ["MusicSynchronizer.Api/MusicSynchronizer.Api.csproj", "MusicSynchronizer.Api/"]
COPY ["MusicSynchronizer.Common/MusicSynchronizer.Common.csproj", "MusicSynchronizer.Common/"]
COPY ["MusicSynchronizer.Domain/MusicSynchronizer.Domain.csproj", "MusicSynchronizer.Domain/"]
COPY ["MusicSynchronizer.Infrastructure/MusicSynchronizer.Infrastructure.csproj", "MusicSynchronizer.Infrastructure/"]
COPY ["MusicSynchronizer.Service/MusicSynchronizer.Service.csproj", "MusicSynchronizer.Service/"]
RUN dotnet restore "./MusicSynchronizer.WebApiHost/MusicSynchronizer.WebApiHost.csproj"
COPY . .
WORKDIR "/src/MusicSynchronizer.WebApiHost"
RUN dotnet build "./MusicSynchronizer.WebApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MusicSynchronizer.WebApiHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicSynchronizer.WebApiHost.dll"]