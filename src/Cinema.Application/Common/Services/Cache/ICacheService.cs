namespace Cinema.Application.Common.Services.Cache;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, 
        CancellationToken cancellationToken = default);
}
