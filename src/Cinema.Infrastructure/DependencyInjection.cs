using Cinema.Application.Common.Interfaces;
using Cinema.Application.Common.Services.Cache;
using Cinema.Infrastructure.Auditoriums.Persistence;
using Cinema.Infrastructure.Common.Persistence;
using Cinema.Infrastructure.Common.Services;
using Cinema.Infrastructure.Movies.Persistence;
using Cinema.Infrastructure.Seats.Persistence;
using Cinema.Infrastructure.Showtimes.Persistence;
using Cinema.Infrastructure.Tickets.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cinema.Infrastructure;

public static  class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CinemaDbContext>(options =>
        {
            options.UseInMemoryDatabase("CinemaDb")
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            //options.UseSqlServer(configuration.GetConnectionString("CinemaDb"));
        });
        
        services.AddSingleton<ICacheService, CacheService>();
        
        services.AddScoped<IAuditoriumsRepository, AuditoriumsRepository>();
        services.AddScoped<IShowtimesRepository, ShowtimesRepository>();
        services.AddScoped<ITicketsRepository, TicketsRepository>();
        services.AddScoped<IMoviesRepository, MoviesRepository>();
        services.AddScoped<ISeatsRepository, SeatsRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<CinemaDbContext>());
        
       AddRedisCache(services, configuration);
        
        return services;
    }
    
    private static void AddRedisCache(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
            configuration.GetSection("Redis:ConnectionString").Value!));
    }
}