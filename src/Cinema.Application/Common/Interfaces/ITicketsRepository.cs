using System.Linq.Expressions;

namespace Cinema.Application.Common.Interfaces;

public interface ITicketsRepository
{
    Task<Ticket?> GetAsync(Guid id, CancellationToken cancellationToken);
    
    Task<List<Ticket>> GetTicketsAsync(Expression<Func<Ticket, bool>> predicate,
        CancellationToken cancellationToken);
    Task RemoveTicketsAsync(IEnumerable<Ticket> tickets, CancellationToken cancellationToken);
    Task AddTicketAsync(Ticket ticket, CancellationToken cancellationToken);
}