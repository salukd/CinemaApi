using System.Linq.Expressions;
using Cinema.Application.Common.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Tickets.Persistence;

public class TicketsRepository : ITicketsRepository
{
    private readonly CinemaDbContext _database;

    public TicketsRepository(CinemaDbContext database)
    {
        _database = database;
    }

    public async Task<Ticket?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _database.Tickets.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }


    public async Task<List<Ticket>> GetTicketsAsync(Expression<Func<Ticket, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await _database.Tickets
            .Include(i => i.Seats)
            .Include(i => i.Showtime).ThenInclude(i=>i.Movie)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public Task RemoveTicketsAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken)
    {
        foreach (var ticket in tickets)
        {
            _database.Tickets.Remove(ticket);
        }

        return Task.CompletedTask;
    }

    public async Task AddTicketAsync(Ticket ticket,
        CancellationToken cancellationToken)
    {
        await _database.Tickets.AddAsync(ticket, cancellationToken);
    }
}