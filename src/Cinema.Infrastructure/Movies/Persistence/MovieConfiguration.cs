using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Movies.Persistence;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(entry => entry.Id);
        builder.Property(entry => entry.Id).ValueGeneratedOnAdd();
        builder.Property(entry => entry.Title).HasMaxLength(50);
        builder.Property(entry => entry.ImdbId).HasMaxLength(20);
    }
}