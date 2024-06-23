namespace Cinema.Domain.Entities;

public class Seat
{
    public Guid? TicketId { get; set; }
    public int Row { get; set; }
    public int SeatNumber { get; set; }
    public int AuditoriumId { get; set; }
    public Ticket? Ticket { get; set; }
    public Auditorium Auditorium { get; set; }

    public override string ToString()
    {
        return $"row: {Row} seat: {SeatNumber}";
    }
}