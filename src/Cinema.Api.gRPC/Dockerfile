# Stage 1: Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install ca-certificates to ensure the root certificates are updated
RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Copy the project files and restore dependencies
COPY ../Cinema.Api.gRPC/Cinema.Api.gRPC.csproj Cinema.Api.gRPC/
COPY ../Cinema.Application/Cinema.Application.csproj Cinema.Application/
COPY ../Cinema.Domain/Cinema.Domain.csproj Cinema.Domain/
COPY ../Cinema.Infrastructure/Cinema.Infrastructure.csproj Cinema.Infrastructure/
RUN dotnet restore "Cinema.Api.gRPC/Cinema.Api.gRPC.csproj"

# Copy the rest of the source code
COPY ../ .

# Build the project
WORKDIR "/src/Cinema.Api.gRPC"
RUN dotnet build "Cinema.Api.gRPC.csproj" -c Release -o /app/build

# Stage 3: Publish stage
FROM build AS publish
RUN dotnet publish "Cinema.Api.gRPC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cinema.Api.gRPC.dll"]
