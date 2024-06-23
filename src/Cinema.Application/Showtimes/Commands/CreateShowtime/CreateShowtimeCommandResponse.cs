namespace Cinema.Application.Showtimes.Commands.CreateShowtime;

public record CreateShowtimeCommandResponse(int Id, DateTime SessionDate, string MovieTitle);