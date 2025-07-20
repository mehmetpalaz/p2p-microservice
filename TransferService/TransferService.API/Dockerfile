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

# Copy TransferService projects
COPY TransferService/TransferService.API/ TransferService/TransferService.API/
COPY TransferService/TransferService.Application/ TransferService/TransferService.Application/
COPY TransferService/TransferService.Domain/ TransferService/TransferService.Domain/
COPY TransferService/TransferService.Infrastructure/ TransferService/TransferService.Infrastructure/
COPY TransferService/TransferService.Persistence/ TransferService/TransferService.Persistence/

RUN ls -la /src/Contracts

# API projesinin bağımlılıklarını restore et
RUN dotnet restore "TransferService/TransferService.API/TransferService.API.csproj"

# Build
WORKDIR /src/TransferService/TransferService.API
RUN dotnet build "TransferService.API.csproj" -c Release -o /app/build

# --------------------
# 3. PUBLISH STAGE
# --------------------
FROM build AS publish
RUN dotnet publish "TransferService.API.csproj" -c Release -o /app/publish

# --------------------
# 4. FINAL IMAGE
# --------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Uygulamayı çalıştır
ENTRYPOINT ["dotnet", "TransferService.API.dll"]
