namespace Cinema.Application.Common.Interfaces;

public interface IAuditoriumsRepository
{
    Task<Auditorium?> GetAsync(int auditoriumId, CancellationToken cancellationToken);
}