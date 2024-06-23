namespace Cinema.Application.Common.Interfaces.Caching;

public interface ICachedQuery
{ 
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}

public interface ICachedQuery<out TResponse> : IQuery<TResponse>, ICachedQuery
{
}