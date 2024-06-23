using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Seats.Persistence;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(entry => new { entry.AuditoriumId, entry.Row, entry.SeatNumber });
        builder.Property(p => p.TicketId);
        builder.HasOne(entry => entry.Auditorium)
            .WithMany(entry => entry.Seats)
            .HasForeignKey(entry => entry.AuditoriumId);

        builder.HasOne(s => s.Ticket)
            .WithMany(t => t.Seats)
            .HasForeignKey(s => s.TicketId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}