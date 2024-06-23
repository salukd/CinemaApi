using Cinema.Application.Common.Exceptions;

namespace Cinema.Application.Tickets.Commands.PurchaseTicket;

public record PurchaseTicketCommand(Guid ReservationId) : IRequest;

public class PurchaseTicketCommandHandler : IRequestHandler<PurchaseTicketCommand>
{
    private readonly ITicketsRepository _ticketsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseTicketCommandHandler(ITicketsRepository ticketsRepository,
        IUnitOfWork unitOfWork)
    {
        _ticketsRepository = ticketsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketsRepository.GetAsync(request.ReservationId, cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException("Reservation not found!");
        }

        if (ticket.Status is TicketStatus.Paid)
        {
            throw new ConflictException("Already purchased!");
        }

        ticket.Status = TicketStatus.Paid;
        ticket.CreatedTime = DateTime.Now;

        await _unitOfWork.CommitChangesAsync(cancellationToken);
    }
}