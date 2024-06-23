using Cinema.Application.Tickets.Commands.CreateReservation;
using Cinema.Application.Tickets.Commands.PurchaseTicket;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Cinema.gRPC.Api.Services;

public class TicketService : TicketsApi.TicketsApiBase
{
    private readonly ISender _mediator;

    public TicketService(ISender mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateReservationResponse> CreateReservation(CreateReservationRequest request, ServerCallContext context)
    {
        var reservation = await _mediator.Send(new CreateReservationCommand(request.ShowtimeId,
            request.Row, request.Seats.ToList()), context.CancellationToken);

        return new CreateReservationResponse
        {
            Id = reservation.Id.ToString(),
            MovieTitle = reservation.MovieTitle,
            AuditoriumId = reservation.AuditoriumId,
            SeatsCount = reservation.SeatsCount
        };
    }

    public override async Task<Empty> PurchaseTicket(PurchaseTicketRequest request, ServerCallContext context)
    {
        await _mediator.Send(new PurchaseTicketCommand(Guid.Parse(request.ReservationId)), 
            context.CancellationToken);

        return new Empty();
    }
}