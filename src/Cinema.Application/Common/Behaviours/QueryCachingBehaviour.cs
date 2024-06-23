using Cinema.Application.Common.Interfaces.Caching;
using Cinema.Application.Common.Services.Cache;

namespace Cinema.Application.Common.Behaviours;

public sealed class QueryCachingBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    private readonly ICacheService _cacheService;

    public QueryCachingBehaviour(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await _cacheService.GetOrCreateAsync(request.CacheKey, 
            _ => next(), 
            request.Expiration,
            cancellationToken);
    }
}