using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Tickets.Persistence;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(entry => entry.Id);
        builder.Property(entry => entry.Id).ValueGeneratedOnAdd();
        builder.Property(o => o.Status).HasConversion(
            v => v.ToString(),
            v => (TicketStatus)Enum.Parse(typeof(TicketStatus), v))
            .IsRequired()
            .HasMaxLength(20);
    }
}