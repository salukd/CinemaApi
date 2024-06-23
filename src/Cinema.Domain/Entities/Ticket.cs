namespace Cinema.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public int ShowtimeId { get; set; }
    public ICollection<Seat> Seats { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public TicketStatus Status { get; set; }
    public Showtime Showtime { get; set; }
}

public enum TicketStatus
{
    Reserved,
    Paid
}