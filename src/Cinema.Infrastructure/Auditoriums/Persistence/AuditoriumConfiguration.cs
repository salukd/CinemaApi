using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Auditoriums.Persistence;

public class AuditoriumConfiguration : IEntityTypeConfiguration<Auditorium>
{
    public void Configure(EntityTypeBuilder<Auditorium> builder)
    {
        builder.HasKey(entry => entry.Id);
        builder.Property(entry => entry.Id).ValueGeneratedOnAdd();
        builder.HasMany(entry => entry.Showtimes).WithOne()
            .HasForeignKey(entity => entity.AuditoriumId);
    }
}