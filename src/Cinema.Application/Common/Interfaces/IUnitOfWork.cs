namespace Cinema.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync(CancellationToken cancellationToken);
}