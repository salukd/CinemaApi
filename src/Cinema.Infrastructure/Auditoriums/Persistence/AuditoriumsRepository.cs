using Cinema.Application.Common.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Auditoriums.Persistence;

public class AuditoriumsRepository : IAuditoriumsRepository
{
    private readonly CinemaDbContext _database;

    public AuditoriumsRepository(CinemaDbContext database)
    {
        _database = database;
    }

    public async Task<Auditorium?> GetAsync(int auditoriumId, CancellationToken cancellationToken)
    {
        return await _database.Auditoriums
            .Include(x => x.Seats)
            .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancellationToken);
    }
}