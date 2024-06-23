namespace Cinema.Contracts.Tickets;

public record CreateReservationRequest(int ShowtimeId, SeatRequest Seats);

public record SeatRequest(int Row, List<int> SeatNumbers);