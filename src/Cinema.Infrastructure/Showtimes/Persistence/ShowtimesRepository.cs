using System.Linq.Expressions;
using Cinema.Application.Common.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Showtimes.Persistence;

public class ShowtimesRepository : IShowtimesRepository
{
    private readonly CinemaDbContext _database;

    public ShowtimesRepository(CinemaDbContext database)
    {
        _database = database;
    }

    public async Task<Showtime?> GetWithMoviesByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _database.Showtimes
            .AsNoTracking()
            .Include(x => x.Movie)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<List<Showtime>> GetAllAsync(Expression<Func<Showtime, bool>>? filter,
        CancellationToken cancellationToken)
    {
        if (filter == null)
        {
            return await _database.Showtimes
                .Include(i => i.Movie)
                .ToListAsync(cancellationToken);
        }
        return await _database.Showtimes
            .Include(i => i.Movie)
            .Where(filter)
            .ToListAsync(cancellationToken);
    }

    public async Task AddShowtimeAsync(Showtime showtime, CancellationToken cancellationToken)
    {
        await _database.Showtimes.AddAsync(showtime, cancellationToken);
    }
}