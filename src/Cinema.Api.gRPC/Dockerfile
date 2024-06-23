FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Cinema.Api.gRPC/Cinema.Api.gRPC.csproj", "src/Cinema.Api.gRPC/"]
COPY ["src/Cinema.Application/Cinema.Application.csproj", "src/Cinema.Application/"]
COPY ["src/Cinema.Domain/Cinema.Domain.csproj", "src/Cinema.Domain/"]
COPY ["src/Cinema.Infrastructure/Cinema.Infrastructure.csproj", "src/Cinema.Infrastructure/"]
RUN dotnet restore "src/Cinema.Api.gRPC/Cinema.Api.gRPC.csproj"
COPY . .
WORKDIR "/src/src/Cinema.Api.gRPC"
RUN dotnet build "Cinema.Api.gRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cinema.Api.gRPC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cinema.Api.gRPC.dll"]
