namespace Cinema.Contracts.Showtimes;

public record CreateShowtimeResponse(int Id, DateTime SessionDate, string MovieTitle);