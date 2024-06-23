using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Showtimes.Persistence;

public class ShowtimeConfiguration : IEntityTypeConfiguration<Showtime>
{
    public void Configure(EntityTypeBuilder<Showtime> builder)
    {
        builder.HasKey(entry => entry.Id);
        builder.Property(entry => entry.Id).ValueGeneratedOnAdd();
        
        builder.HasOne(entry => entry.Movie)
            .WithMany(entry => entry.Showtimes)
            .HasForeignKey(s => s.MovieId);
        
        builder.HasMany(entry => entry.Tickets)
            .WithOne(entry => entry.Showtime)
            .HasForeignKey(entry => entry.ShowtimeId);
        
    }
}