namespace Cinema.Domain.Entities;

public class Showtime
{
    public int Id { get; set; }
    public Movie Movie { get; set; }
    public int MovieId { get; set; }
    public DateTime SessionDate { get; set; }
    public int AuditoriumId { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}