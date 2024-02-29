
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/driver/Ride23.Driver.API/Ride23.Driver.API.csproj", "src/services/driver/Ride23.Driver.API/"]
COPY ["src/services/driver/Ride23.Driver.Application/Ride23.Driver.Application.csproj", "src/services/driver/Ride23.Driver.Application/"]
COPY ["framework/Ride23.Framework.Core/Ride23.Framework.Core.csproj", "framework/Ride23.Framework.Core/"]
COPY ["src/events/Ride23.Event/Ride23.Event.csproj", "src/events/Ride23.Event/"]
COPY ["src/services/driver/Ride23.Driver.Domain/Ride23.Driver.Domain.csproj", "src/services/driver/Ride23.Driver.Domain/"]
COPY ["src/services/driver/Ride23.Driver.Infrastructure/Ride23.Driver.Infrastructure.csproj", "src/services/driver/Ride23.Driver.Infrastructure/"]
COPY ["framework/Ride23.Framework.Infrastructure/Ride23.Framework.Infrastructure.csproj", "framework/Ride23.Framework.Infrastructure/"]
COPY ["framework/Ride23.Framework.Persistence.EFCore/Ride23.Framework.Persistence.EFCore.csproj", "framework/Ride23.Framework.Persistence.EFCore/"]
RUN dotnet restore "./src/services/driver/Ride23.Driver.API/Ride23.Driver.API.csproj"
COPY . .
WORKDIR "/src/src/services/driver/Ride23.Driver.API"
RUN dotnet build "./Ride23.Driver.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ride23.Driver.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ride23.Driver.API.dll"]