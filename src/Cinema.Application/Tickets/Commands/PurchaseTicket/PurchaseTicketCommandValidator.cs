using FluentValidation;

namespace Cinema.Application.Tickets.Commands.PurchaseTicket;

public class PurchaseTicketCommandValidator : AbstractValidator<PurchaseTicketCommand>
{
    public PurchaseTicketCommandValidator()
    {
        RuleFor(r => r.ReservationId).NotEmpty();
    }
}