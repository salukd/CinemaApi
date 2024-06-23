using Cinema.Application.Common.Exceptions;

namespace Cinema.Application.Tickets.Commands.CreateReservation;

public record CreateReservationCommand(int ShowtimeId, int Row, List<int> Seats) :
    IRequest<CreateReservationCommandResponse>;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, CreateReservationCommandResponse>
{
    private readonly IShowtimesRepository _showtimesRepository;
    private readonly IAuditoriumsRepository _auditoriumsRepository;
    private readonly ITicketsRepository _ticketsRepository;
    private readonly ISeatsRepository _seatsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservationCommandHandler(
        IShowtimesRepository showtimesRepository,
        IAuditoriumsRepository auditoriumsRepository,
        ITicketsRepository ticketsRepository,
        ISeatsRepository seatsRepository,
        IUnitOfWork unitOfWork)
    {
        _showtimesRepository = showtimesRepository;
        _auditoriumsRepository = auditoriumsRepository;
        _ticketsRepository = ticketsRepository;
        _seatsRepository = seatsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateReservationCommandResponse> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _showtimesRepository.GetWithMoviesByIdAsync(request.ShowtimeId, cancellationToken);
        if (showtime is null)
        {
            throw new NotFoundException("Showtime not found!");
        }

        var auditorium = await _auditoriumsRepository.GetAsync(showtime.AuditoriumId, cancellationToken);
        if (auditorium is null)
        {
            throw new NotFoundException("Auditorium not found!");
        }

        var seats = await _seatsRepository.GetSeatsByIdsAsync(auditorium.Id, request.Row, request.Seats, cancellationToken);
        if (seats.Any(seat => seat.TicketId.HasValue))
        {
            throw new ConflictException("Some seats are already reserved or sold");
        }

        var ticket = CreateTicket(showtime.Id);
        await SaveReservationAsync(ticket, auditorium.Id, request.Row, request.Seats, cancellationToken);

        return new CreateReservationCommandResponse(ticket.Id, 
            auditorium.Id, 
            seats.Count, 
            showtime.Movie.Title);
    }
    
    private static Ticket CreateTicket(int showtimeId) =>
        new()
        {
            Id = Guid.NewGuid(),
            Status = TicketStatus.Reserved,
            ShowtimeId = showtimeId
        };

    private async Task SaveReservationAsync(Ticket ticket, int auditoriumId, int row, List<int> seatIds, CancellationToken cancellationToken)
    {
        await _ticketsRepository.AddTicketAsync(ticket, cancellationToken);
        await _seatsRepository.UpdateSeatsAsync(ticket.Id, auditoriumId, row, seatIds, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
    }
}
