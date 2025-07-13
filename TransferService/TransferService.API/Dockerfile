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

# Tüm projeleri ve kaynak dosyaları kopyala
COPY TransferService/TransferService.API/ TransferService.API/
COPY TransferService/TransferService.Application/ TransferService.Application/
COPY TransferService/TransferService.Domain/ TransferService.Domain/
COPY TransferService/TransferService.Infrastructure/ TransferService.Infrastructure/
COPY TransferService/TransferService.Persistence/ TransferService.Persistence/

# API projesinin bağımlılıklarını restore et
RUN dotnet restore "TransferService.API/TransferService.API.csproj"

# Build
WORKDIR /src/TransferService.API
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
