
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/inventory/Ride23.Inventory.API/Ride23.Inventory.API.csproj", "src/services/inventory/Ride23.Inventory.API/"]
COPY ["src/services/inventory/Ride23.Inventory.Application/Ride23.Inventory.Application.csproj", "src/services/inventory/Ride23.Inventory.Application/"]
COPY ["framework/Ride23.Framework.Core/Ride23.Framework.Core.csproj", "framework/Ride23.Framework.Core/"]
COPY ["src/events/Ride23.Event/Ride23.Event.csproj", "src/events/Ride23.Event/"]
COPY ["src/sagas/Ride23.Saga/Ride23.Saga.csproj", "src/sagas/Ride23.Saga/"]
COPY ["src/services/inventory/Ride23.Inventory.Domain/Ride23.Inventory.Domain.csproj", "src/services/inventory/Ride23.Inventory.Domain/"]
COPY ["src/services/inventory/Ride23.Inventory.Infrastructure/Ride23.Inventory.Infrastructure.csproj", "src/services/inventory/Ride23.Inventory.Infrastructure/"]
COPY ["framework/Ride23.Framework.Persistence.EFCore/Ride23.Framework.Persistence.EFCore.csproj", "framework/Ride23.Framework.Persistence.EFCore/"]
COPY ["framework/Ride23.Framework.Infrastructure/Ride23.Framework.Infrastructure.csproj", "framework/Ride23.Framework.Infrastructure/"]
RUN dotnet restore "./src/services/inventory/Ride23.Inventory.API/Ride23.Inventory.API.csproj"
COPY . .
WORKDIR "/src/src/services/inventory/Ride23.Inventory.API"
RUN dotnet build "./Ride23.Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ride23.Inventory.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ride23.Inventory.API.dll"]