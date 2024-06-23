namespace Cinema.Contracts.Showtimes;

public record CreateShowtimeRequest(int AuditoriumId, int MovieId, DateTime SessionDate);
