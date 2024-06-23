namespace Cinema.Contracts.Tickets;

public record GetTicketsResponse(
    Guid Id,
    int AuditoriumId,
    string MovieTitle,
    List<string> Seats,
    string ExpiresInMinutes);