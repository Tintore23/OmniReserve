# 1. Etapa de ejecución (Base)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Etapa de compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar los archivos de proyectos manteniendo la estructura
COPY ["OmniReserve.api/OmniReserve.api.csproj", "OmniReserve.api/"]
COPY ["OmniReserve.Infrastructure/OmniReserve.Infrastructure.csproj", "OmniReserve.Infrastructure/"]
COPY ["OmniReserve.Application/OmniReserve.Application.csproj", "OmniReserve.Application/"]
COPY ["OmniReserve.Domain/OmniReserve.Domain.csproj", "OmniReserve.Domain/"]

# Restaurar dependencias
RUN dotnet restore "OmniReserve.api/OmniReserve.api.csproj"

# Copiar el resto del código fuente
COPY . .
WORKDIR "/src/OmniReserve.api"
RUN dotnet build "OmniReserve.api.csproj" -c Release -o /app/build

# 3. Etapa de publicación
FROM build AS publish
RUN dotnet publish "OmniReserve.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OmniReserve.api.dll"]