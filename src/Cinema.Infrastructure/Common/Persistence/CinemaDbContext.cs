using System.Reflection;
using Cinema.Application.Common.Interfaces;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Common.Persistence;

public class CinemaDbContext : DbContext, IUnitOfWork
{
    public DbSet<Auditorium> Auditoriums { get; set; } = null!;
    public DbSet<Seat> Seats { get; set; } = null!;
    public DbSet<Showtime> Showtimes { get; set; } = null!;
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options) { }
    
    public async Task CommitChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}