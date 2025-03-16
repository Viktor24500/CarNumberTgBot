# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CarNumberRegionsTgBot/CarNumberRegionsTgBot.csproj", "CarNumberRegionsTgBot/"]
RUN dotnet restore "./CarNumberRegionsTgBot/CarNumberRegionsTgBot.csproj"
COPY . .
WORKDIR "/src/CarNumberRegionsTgBot"
RUN dotnet build "./CarNumberRegionsTgBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN mkdir -p /app/Properties
COPY CarNumberRegionsTgBot/Properties /app/Properties

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarNumberRegionsTgBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /app/Properties /app/Properties
ENTRYPOINT ["dotnet", "CarNumberRegionsTgBot.dll"]