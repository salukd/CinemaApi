using System.Linq.Expressions;

namespace Cinema.Application.Common.Interfaces;

public interface IShowtimesRepository
{
    Task<Showtime?> GetWithMoviesByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<List<Showtime>> GetAllAsync(Expression<Func<Showtime, bool>>? filter,
        CancellationToken cancellationToken);

    Task AddShowtimeAsync(Showtime showtime, CancellationToken cancellationToken);
}