namespace Cinema.Domain.Entities;

public class Auditorium
{
    public int Id { get; set; }
    public List<Showtime> Showtimes { get; set; }
    public ICollection<Seat> Seats { get; set; }

    public int GetNumberOfSeats() => Seats.Max(m => m.Row) * Seats.Max(m => m.SeatNumber);
}