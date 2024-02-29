
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/customer/Ride23.Customer.API/Ride23.Customer.API.csproj", "src/services/customer/Ride23.Customer.API/"]
COPY ["src/services/customer/Ride23.Customer.Application/Ride23.Customer.Application.csproj", "src/services/customer/Ride23.Customer.Application/"]
COPY ["framework/Ride23.Framework.Core/Ride23.Framework.Core.csproj", "framework/Ride23.Framework.Core/"]
COPY ["src/events/Ride23.Event/Ride23.Event.csproj", "src/events/Ride23.Event/"]
COPY ["src/services/customer/Ride23.Customer.Domain/Ride23.Customer.Domain.csproj", "src/services/customer/Ride23.Customer.Domain/"]
COPY ["src/services/customer/Ride23.Customer.Infrastructure/Ride23.Customer.Infrastructure.csproj", "src/services/customer/Ride23.Customer.Infrastructure/"]
COPY ["framework/Ride23.Framework.Persistence.EFCore/Ride23.Framework.Persistence.EFCore.csproj", "framework/Ride23.Framework.Persistence.EFCore/"]
COPY ["framework/Ride23.Framework.Infrastructure/Ride23.Framework.Infrastructure.csproj", "framework/Ride23.Framework.Infrastructure/"]
RUN dotnet restore "./src/services/customer/Ride23.Customer.API/Ride23.Customer.API.csproj"
COPY . .
WORKDIR "/src/src/services/customer/Ride23.Customer.API"
RUN dotnet build "./Ride23.Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ride23.Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ride23.Customer.API.dll"]