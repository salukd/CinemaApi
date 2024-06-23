using Cinema.Application.Common.Interfaces.Caching;

namespace Cinema.Application.Showtimes.Queries.GetAll;

public record GetAllShowtimeQuery : ICachedQuery<List<Showtime>>
{
    public string CacheKey => "get-all-showtime-query";
    public TimeSpan? Expiration => TimeSpan.FromSeconds(20);
}

public class GetAllShowtimeQueryHandler : IRequestHandler<GetAllShowtimeQuery, List<Showtime>>
{
    private readonly IShowtimesRepository _showtimesRepository;

    public GetAllShowtimeQueryHandler(IShowtimesRepository showtimesRepository)
    {
        _showtimesRepository = showtimesRepository;
    }

    public async Task<List<Showtime>> Handle(GetAllShowtimeQuery request, CancellationToken cancellationToken)
    {
        return await _showtimesRepository.GetAllAsync(null, cancellationToken);
    }
}