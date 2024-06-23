using Cinema.Application.Common.Exceptions;

namespace Cinema.Application.Showtimes.Commands.CreateShowtime;

public record CreateShowtimeCommand(int AuditoriumId, int MovieId, DateTime SessionDate) : 
    IRequest<CreateShowtimeCommandResponse>;

public class CreateShowtimeCommandHandler : 
    IRequestHandler<CreateShowtimeCommand, CreateShowtimeCommandResponse>
{
    private readonly IShowtimesRepository _showtimesRepository;
    private readonly IMoviesRepository _moviesRepository;
    
    private readonly IUnitOfWork _unitOfWork;

    private const string ErrorMessage = "Error while creating showtime";

    public CreateShowtimeCommandHandler(
        IShowtimesRepository showtimesRepository,
        IMoviesRepository moviesRepository, 
        
        IUnitOfWork unitOfWork)
    {
        _showtimesRepository = showtimesRepository;
        _moviesRepository = moviesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateShowtimeCommandResponse> Handle(CreateShowtimeCommand request, 
        CancellationToken cancellationToken)
    {
        if (await ShowtimeExistsAsync(request, cancellationToken))
        {
            throw new ConflictException("Showtime already exists");
        }

        var movie = await _moviesRepository.GetMovieByIdAsync(request.MovieId, cancellationToken);

        if (movie is null)
        {
            throw new NotFoundException("Movie not found");
        }

        var showtime = CreateShowtime(request, movie);

        await SaveShowtimeAsync(showtime, cancellationToken);

        return new CreateShowtimeCommandResponse(showtime.Id, showtime.SessionDate, movie.Title);
        
    }

    private async Task SaveShowtimeAsync(Showtime showtime, CancellationToken cancellationToken)
    {
        await _showtimesRepository.AddShowtimeAsync(showtime, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    private static Showtime CreateShowtime(CreateShowtimeCommand request, Movie movie)
        => new()
        {
            AuditoriumId = request.AuditoriumId,
            MovieId = request.MovieId,
            SessionDate = request.SessionDate
        };


    private async Task<bool> ShowtimeExistsAsync(CreateShowtimeCommand request, CancellationToken cancellationToken)
    {
        var existingShowtimes = await _showtimesRepository.GetAllAsync(
            s => s.AuditoriumId == request.AuditoriumId && 
                 s.SessionDate.Date == request.SessionDate.Date && 
                 s.Movie.Id == request.MovieId,
            cancellationToken);

        return existingShowtimes.Any();
    }
}