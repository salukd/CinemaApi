using Cinema.Application.Common.Behaviours;
using Cinema.Application.ExpiredReservationsWorker;
using Coravel;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.Application;

public static  class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            config.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            config.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            config.AddOpenBehavior(typeof(QueryCachingBehaviour<,>));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }

    public static IServiceCollection AddWorker(this IServiceCollection services)
    {
        services.AddScheduler();
        services.AddTransient<RemoveExpiredReservations>();

        return services;
    }
}