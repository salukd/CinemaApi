namespace Cinema.Application.Tickets.Commands.CreateReservation;

public record CreateReservationCommandResponse(Guid Id, int AuditoriumId, int SeatsCount, string MovieTitle);
