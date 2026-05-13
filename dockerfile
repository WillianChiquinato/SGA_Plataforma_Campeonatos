FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SGA_Plataforma.Api/SGA_Plataforma.Api.csproj", "SGA_Plataforma.Api/"]
COPY ["SGA_Plataforma.Infrastructure/SGA_Plataforma.Infrastructure.csproj", "SGA_Plataforma.Infrastructure/"]
COPY ["SGA_Plataforma.Contracts/SGA_Plataforma.Contracts.csproj", "SGA_Plataforma.Contracts/"]
RUN dotnet restore "SGA_Plataforma.Api/SGA_Plataforma.Api.csproj"
COPY . .
WORKDIR "/src/SGA_Plataforma.Api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

# Atualizado para a porta padrão do .NET 8
EXPOSE 8080

# Healthcheck apontando para a 8080
HEALTHCHECK --interval=30s --timeout=5s --start-period=40s --retries=5 \
    CMD curl -f http://localhost:8080/ || exit 1

ENTRYPOINT ["dotnet", "SGA_Plataforma.Api.dll"]