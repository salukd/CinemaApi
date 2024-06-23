namespace Cinema.Contracts.Tickets;

public record CreateReservationResponse(Guid ReservationId, int Seats, int Auditorium, string MovieTitle);
