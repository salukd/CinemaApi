using Cinema.Application.Common.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Movies.Persistence;

public class MoviesRepository : IMoviesRepository
{
    private readonly CinemaDbContext _database;

    public MoviesRepository(CinemaDbContext database)
    {
        _database = database;
    }

    public async Task<List<Movie>> GetMoviesAsync(CancellationToken cancellationToken)
    {
        return await _database.Movies.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Movie?> GetMovieByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _database.Movies.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }
}