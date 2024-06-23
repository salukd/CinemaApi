using Cinema.Application.Common.Interfaces.Caching;

namespace Cinema.Application.Showtimes.Queries.GetFiltered;

public record GetFilteredShowtimeQuery(string Query) : ICachedQuery<List<Showtime>>
{
    public string CacheKey => $"get-filtered-showtime-query-{Query}";
    public TimeSpan? Expiration => TimeSpan.FromSeconds(20);
}

public class GetFilteredShowtimeQueryHandler : IRequestHandler<GetFilteredShowtimeQuery, List<Showtime>>
{
    private readonly IShowtimesRepository _showtimesRepository;

    public GetFilteredShowtimeQueryHandler(IShowtimesRepository showtimesRepository)
    {
        _showtimesRepository = showtimesRepository;
    }
    

    public async Task<List<Showtime>> Handle(GetFilteredShowtimeQuery request, CancellationToken cancellationToken)
    {
        return await _showtimesRepository.GetAllAsync(showtime=>
                !string.IsNullOrEmpty(showtime.Movie.Title) && showtime.Movie.Title.Contains(request.Query,
                    StringComparison.OrdinalIgnoreCase)
            , cancellationToken);
    }
}