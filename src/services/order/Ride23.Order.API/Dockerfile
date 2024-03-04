
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/order/Ride23.Order.API/Ride23.Order.API.csproj", "src/services/order/Ride23.Order.API/"]
COPY ["src/services/order/Ride23.Order.Application/Ride23.Order.Application.csproj", "src/services/order/Ride23.Order.Application/"]
COPY ["framework/Ride23.Framework.Core/Ride23.Framework.Core.csproj", "framework/Ride23.Framework.Core/"]
COPY ["src/events/Ride23.Event/Ride23.Event.csproj", "src/events/Ride23.Event/"]
COPY ["src/sagas/Ride23.Saga/Ride23.Saga.csproj", "src/sagas/Ride23.Saga/"]
COPY ["src/services/order/Ride23.Order.Domain/Ride23.Order.Domain.csproj", "src/services/order/Ride23.Order.Domain/"]
COPY ["src/services/order/Ride23.Order.Infrastructure/Ride23.Order.Infrastructure.csproj", "src/services/order/Ride23.Order.Infrastructure/"]
COPY ["framework/Ride23.Framework.Persistence.EFCore/Ride23.Framework.Persistence.EFCore.csproj", "framework/Ride23.Framework.Persistence.EFCore/"]
COPY ["framework/Ride23.Framework.Infrastructure/Ride23.Framework.Infrastructure.csproj", "framework/Ride23.Framework.Infrastructure/"]
RUN dotnet restore "./src/services/order/Ride23.Order.API/Ride23.Order.API.csproj"
COPY . .
WORKDIR "/src/src/services/order/Ride23.Order.API"
RUN dotnet build "./Ride23.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ride23.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ride23.Order.API.dll"]