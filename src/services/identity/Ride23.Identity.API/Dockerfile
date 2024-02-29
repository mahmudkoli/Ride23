
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/identity/Ride23.Identity.API/Ride23.Identity.API.csproj", "src/services/identity/Ride23.Identity.API/"]
COPY ["src/services/identity/Ride23.Identity.Infrastructure/Ride23.Identity.Infrastructure.csproj", "src/services/identity/Ride23.Identity.Infrastructure/"]
COPY ["framework/Ride23.Framework.Infrastructure/Ride23.Framework.Infrastructure.csproj", "framework/Ride23.Framework.Infrastructure/"]
COPY ["framework/Ride23.Framework.Core/Ride23.Framework.Core.csproj", "framework/Ride23.Framework.Core/"]
COPY ["src/services/identity/Ride23.Identity.Application/Ride23.Identity.Application.csproj", "src/services/identity/Ride23.Identity.Application/"]
COPY ["src/services/identity/Ride23.Identity.Domain/Ride23.Identity.Domain.csproj", "src/services/identity/Ride23.Identity.Domain/"]
RUN dotnet restore "./src/services/identity/Ride23.Identity.API/Ride23.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/services/identity/Ride23.Identity.API"
RUN dotnet build "./Ride23.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ride23.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ride23.Identity.API.dll"]