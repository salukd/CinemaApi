using Cinema.Application.Common.Exceptions;
using Cinema.Application.Common.Interfaces.Caching;

namespace Cinema.Application.Showtimes.Queries.GetShowtimeByIdWithMovie;

public record GetShowtimeByIdWithMovieQuery(int Id) : ICachedQuery<Showtime>
{
    public string CacheKey => $"get-showtime-by-id-with-movie-query_{Id}";
    public TimeSpan? Expiration => null;
}

public class GetShowtimeByIdWithMovieQueryHandler : IRequestHandler<GetShowtimeByIdWithMovieQuery, Showtime>
{
    private readonly IShowtimesRepository _showtimesRepository;
    

    public GetShowtimeByIdWithMovieQueryHandler(
        IShowtimesRepository showtimesRepository)
    {
        _showtimesRepository = showtimesRepository;
    }

    public async Task<Showtime> Handle(GetShowtimeByIdWithMovieQuery request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimesRepository.GetWithMoviesByIdAsync(request.Id, cancellationToken);

        if (showtime is null)
        {
            throw new NotFoundException("Showtime not found!");
        }

        return showtime;
    }
}