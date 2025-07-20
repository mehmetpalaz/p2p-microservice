# --------------------
# 1. RUNTIME STAGE
# --------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# --------------------
# 2. BUILD STAGE
# --------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Contracts project with correct path structure
COPY Contracts/Contracts.csproj Contracts/
COPY Contracts/ Contracts/

# Copy NotificationService projects
COPY NotificationService/NotificationService.API/ NotificationService/NotificationService.API/
COPY NotificationService/NotificationService.Domain/ NotificationService/NotificationService.Domain/
COPY NotificationService/NotificationService.Application/ NotificationService/NotificationService.Application/
COPY NotificationService/NotificationService.Infrastructure/ NotificationService/NotificationService.Infrastructure/

RUN ls -la /src/Contracts

# API
RUN dotnet restore "NotificationService/NotificationService.API/NotificationService.API.csproj"

# Build
WORKDIR /src/NotificationService/NotificationService.API
RUN dotnet build "NotificationService.API.csproj" -c Release -o /app/build

# --------------------
# 3. PUBLISH STAGE
# --------------------
FROM build AS publish
RUN dotnet publish "NotificationService.API.csproj" -c Release -o /app/publish

# --------------------
# 4. FINAL IMAGE
# --------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Uygulamayı çalıştır
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
