using System.Linq.Expressions;
using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cinema.Application.ExpiredReservationsWorker;

public class RemoveExpiredReservations : IInvocable
{
    private readonly ITicketsRepository _ticketsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveExpiredReservations> _logger;

    private readonly int _expiredMinutes;

    public RemoveExpiredReservations(ITicketsRepository ticketsRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<RemoveExpiredReservations> logger)
    {
        _ticketsRepository = ticketsRepository;
        _unitOfWork = unitOfWork;
        _expiredMinutes = configuration.GetValue<int>("Worker:ReservationExpireMinutes");
        _logger = logger;
    }

    public async Task Invoke()
    {
        try
        {
            var expiredReservations = await GetExpiredReservationsAsync();

            if (expiredReservations.Any())
            {
                await RemoveExpiredReservationsAsync(expiredReservations);
                _logger.LogInformation($"{expiredReservations.Count} expired reservations removed.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while removing expired reservations.");
        }
    }

    private async Task<List<Ticket>> GetExpiredReservationsAsync()
    {
        var expirationThreshold = DateTime.Now.AddMinutes(-_expiredMinutes);

        Expression<Func<Ticket, bool>> predicate = ticket =>
            ticket.Status == TicketStatus.Reserved && ticket.CreatedTime <= expirationThreshold;


        return await _ticketsRepository.GetTicketsAsync(predicate, CancellationToken.None);
    }

    private async Task RemoveExpiredReservationsAsync(IReadOnlyCollection<Ticket> expiredReservations)
    {
        await _ticketsRepository.RemoveTicketsAsync(expiredReservations, CancellationToken.None);
        await _unitOfWork.CommitChangesAsync(CancellationToken.None);
    }
}