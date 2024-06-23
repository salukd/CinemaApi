using Cinema.Application.Showtimes.Commands.CreateShowtime;
using Cinema.Application.Showtimes.Queries.GetAll;
using Cinema.Application.Showtimes.Queries.GetFiltered;
using Cinema.Application.Showtimes.Queries.GetShowtimeByIdWithMovie;
using Cinema.Contracts.Showtimes;

namespace Cinema.Api.Controllers;

public class ShowtimesController : ApiController
{
    public ShowtimesController(ISender mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(typeof(void), 201)]
    public async Task<IActionResult> CreateShowtime(CreateShowtimeRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateShowtimeCommand(request.AuditoriumId, request.MovieId, request.SessionDate);

        var createShowtimeResult = await Mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetShowtime),
            new { createShowtimeResult.Id },
            new CreateShowtimeResponse(createShowtimeResult.Id, createShowtimeResult.SessionDate.Date,
                createShowtimeResult.MovieTitle));
    }

    [HttpGet("{id:int}/movies")]
    [ProducesResponseType(typeof(GetShowtimeResponse), 200)]
    public async Task<IActionResult> GetShowtime(int id, CancellationToken cancellationToken)
    {
        var showtimeResult = await Mediator.Send(new GetShowtimeByIdWithMovieQuery(id),
            cancellationToken);

        return Ok(new GetShowtimeResponse(showtimeResult.Id,
            showtimeResult.SessionDate.Date,
            showtimeResult.Movie.Title));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GetShowtimeResponse>), 200)]
    public async Task<IActionResult> GetAllShowtime(CancellationToken cancellationToken)
    {
        var showtimeResult = await Mediator.Send(new GetAllShowtimeQuery(),
            cancellationToken);

        return Ok(showtimeResult.ConvertAll(show => new GetShowtimeResponse(show.Id,
            show.SessionDate.Date,
            show.Movie.Title)));
    }
    
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<GetShowtimeResponse>), 200)]
    public async Task<IActionResult> GetFilteredShowtime([FromQuery] string q, CancellationToken cancellationToken)
    {
        var showtimeResult = await Mediator.Send(new GetFilteredShowtimeQuery(q),
            cancellationToken);

        return Ok(showtimeResult.ConvertAll(show => new GetShowtimeResponse(show.Id,
            show.SessionDate.Date,
            show.Movie.Title)));
    }
}