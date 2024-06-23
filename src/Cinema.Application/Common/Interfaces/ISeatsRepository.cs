namespace Cinema.Application.Common.Interfaces;

public interface ISeatsRepository
{
    Task UpdateSeatsAsync(Guid ticketId, int auditoriumId, int row, List<int> seatNumbers, CancellationToken cancellationToken);
    Task<List<Seat>> GetSeatsByIdsAsync(int auditoriumId, int row, List<int> seatNumbers,
        CancellationToken cancellationToken);
}