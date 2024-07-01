FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates \
    && rm -rf /var/lib/apt/lists/*

COPY ../Cinema.Api.Rest/Cinema.Api.Rest.csproj  Cinema.Api.Rest/
COPY ../Cinema.Application/Cinema.Application.csproj Cinema.Application/
COPY ../Cinema.Domain/Cinema.Domain.csproj Cinema.Domain/
COPY ../Cinema.Contracts/Cinema.Contracts.csproj Cinema.Contracts/
COPY ../Cinema.Infrastructure/Cinema.Infrastructure.csproj Cinema.Infrastructure/

RUN dotnet restore "Cinema.Api.Rest/Cinema.Api.Rest.csproj"

COPY . .

WORKDIR "/src/Cinema.Api.Rest"
RUN dotnet build "Cinema.Api.Rest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cinema.Api.Rest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cinema.Api.Rest.dll"]
