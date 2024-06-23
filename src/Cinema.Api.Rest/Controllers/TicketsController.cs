using Cinema.Application.Tickets.Commands.CreateReservation;
using Cinema.Application.Tickets.Commands.PurchaseTicket;
using Cinema.Contracts.Purchases;
using Cinema.Contracts.Tickets;

namespace Cinema.Api.Controllers;

public class TicketsController : ApiController
{
    public TicketsController(ISender mediator) : base(mediator)
    {
    }

    [HttpPost("reservation")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> CreateReservation(CreateReservationRequest request,
        CancellationToken cancellationToken)
    {
        var createReservationResult = await Mediator.Send(new CreateReservationCommand(request.ShowtimeId,
            request.Seats.Row, request.Seats.SeatNumbers), cancellationToken);

        return Ok(
            new CreateReservationResponse(createReservationResult.Id,
                createReservationResult.SeatsCount, 
                createReservationResult.AuditoriumId, 
                createReservationResult.MovieTitle));
    }
    
    [HttpPost("purchase")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> PurchaseTicket(PurchaseTicketRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new PurchaseTicketCommand(request.ReservationId), cancellationToken);

        return Ok();
    }
}