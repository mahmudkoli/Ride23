
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/location/Ride23.Location.API/Ride23.Location.API.csproj", "src/services/location/Ride23.Location.API/"]
COPY ["framework/Ride23.Framework.Core/Ride23.Framework.Core.csproj", "framework/Ride23.Framework.Core/"]
COPY ["framework/Ride23.Framework.Infrastructure/Ride23.Framework.Infrastructure.csproj", "framework/Ride23.Framework.Infrastructure/"]
COPY ["framework/Ride23.Framework.Persistence.NoSQL/Ride23.Framework.Persistence.NoSQL.csproj", "framework/Ride23.Framework.Persistence.NoSQL/"]
COPY ["src/events/Ride23.Event/Ride23.Event.csproj", "src/events/Ride23.Event/"]
RUN dotnet restore "./src/services/location/Ride23.Location.API/Ride23.Location.API.csproj"
COPY . .
WORKDIR "/src/src/services/location/Ride23.Location.API"
RUN dotnet build "./Ride23.Location.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ride23.Location.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ride23.Location.API.dll"]