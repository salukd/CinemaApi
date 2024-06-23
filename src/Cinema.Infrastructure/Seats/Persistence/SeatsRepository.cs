using Cinema.Application.Common.Interfaces;
using Cinema.Domain.Entities;
using Cinema.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Seats.Persistence;

public class SeatsRepository : ISeatsRepository
{
    private readonly CinemaDbContext _database;

    public SeatsRepository(CinemaDbContext database)
    {
        _database = database;
    }
    
    public async Task UpdateSeatsAsync(Guid ticketId, int auditoriumId, int row, List<int> seatNumbers, CancellationToken cancellationToken)
    {
        var seats = await GetSeatsAsync(auditoriumId, row, seatNumbers, cancellationToken);
        seats.ForEach(seat => seat.TicketId = ticketId);
    }

    public async Task<List<Seat>> GetSeatsByIdsAsync(int auditoriumId, int row, List<int> seatNumbers, 
        CancellationToken cancellationToken)
    {
        return await GetSeatsAsync(auditoriumId, row, seatNumbers, cancellationToken);
    }

    private async Task<List<Seat>> GetSeatsAsync(int auditoriumId, int row, List<int> seatNumbers, CancellationToken cancellationToken)
    {
        return await _database.Seats.Where(s => seatNumbers.Contains(s.SeatNumber) && 
                                                s.AuditoriumId == auditoriumId &&
                                                s.Row == row).ToListAsync(cancellationToken);
    }
}