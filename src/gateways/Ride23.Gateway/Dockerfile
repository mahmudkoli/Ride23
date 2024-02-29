
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/gateways/Ride23.Gateway/Ride23.Gateway.csproj", "src/gateways/Ride23.Gateway/"]
COPY ["framework/Ride23.Framework.Infrastructure/Ride23.Framework.Infrastructure.csproj", "framework/Ride23.Framework.Infrastructure/"]
COPY ["framework/Ride23.Framework.Core/Ride23.Framework.Core.csproj", "framework/Ride23.Framework.Core/"]
RUN dotnet restore "./src/gateways/Ride23.Gateway/Ride23.Gateway.csproj"
COPY . .
WORKDIR "/src/src/gateways/Ride23.Gateway"
RUN dotnet build "./Ride23.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ride23.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ride23.Gateway.dll"]