namespace Cinema.Contracts.Showtimes;

public record GetShowtimeResponse(int Id, DateTime SessionDate, string MovieTitle);