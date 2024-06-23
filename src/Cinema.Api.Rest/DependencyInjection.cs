using System.Text.Json.Serialization;
using Asp.Versioning;
namespace Cinema.Api;
using Hellang.Middleware.ProblemDetails;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddExceptionHandler<CustomExceptionHandler>();
        
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails(setup => { setup.IncludeExceptionDetails = (ctx, env) => false; });
        services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ReportApiVersions = true;
        }).AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        return services;
    }
}