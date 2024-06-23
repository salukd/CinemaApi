namespace Cinema.Application.Common.Interfaces;

public interface IMoviesRepository
{
    Task<List<Movie>> GetMoviesAsync(CancellationToken cancellationToken);
    Task<Movie?> GetMovieByIdAsync(int id, CancellationToken cancellationToken);
}