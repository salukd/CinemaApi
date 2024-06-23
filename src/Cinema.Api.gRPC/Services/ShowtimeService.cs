using Cinema.Application.Showtimes.Commands.CreateShowtime;
using Cinema.Application.Showtimes.Queries.GetAll;
using Cinema.Application.Showtimes.Queries.GetFiltered;
using Cinema.Application.Showtimes.Queries.GetShowtimeByIdWithMovie;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Cinema.gRPC.Api.Services;

public class ShowtimeService : Showtime.ShowtimeBase
{
    private readonly ISender _mediator;

    public ShowtimeService(ISender mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CreateShowtimeResponse> CreateShowtime(CreateShowtimeRequest request,
        ServerCallContext context)
    {
        var showtime = await _mediator.Send(new CreateShowtimeCommand(request.MovieId,
            request.MovieId, request.SessionDate.ToDateTime()), context.CancellationToken);

        return new CreateShowtimeResponse
        {
            Id = showtime.Id,
            MovieTitle = showtime.MovieTitle,
            SessionDate = showtime.SessionDate.ToString("o")
        };
    }

    public override async Task<ShowtimeResponse> GetById(IdRequest request, ServerCallContext context)
    {
        var showtime = await _mediator.Send(new GetShowtimeByIdWithMovieQuery(request.Id), 
            context.CancellationToken);

        return new ShowtimeResponse
        {
            Id = showtime.Id,
            SessionDate = showtime.SessionDate.ToString("o"),
            MovieTitle = showtime.Movie.Title
        };
    }

    public override async Task<ShowtimeListResponse> GetAll(Empty request, ServerCallContext context)
    {
        var showtimes = await _mediator.Send(new GetAllShowtimeQuery(), 
            context.CancellationToken);


        return GetResponse(showtimes);
    }

    public override async Task<ShowtimeListResponse> Search(SearchRequest request, ServerCallContext context)
    {
        var showtimes = await _mediator.Send(new GetFilteredShowtimeQuery(request.Text), 
            context.CancellationToken);

        return GetResponse(showtimes);
    }

    private static ShowtimeListResponse GetResponse(List<Domain.Entities.Showtime> showtimes)
    {
        var response = new ShowtimeListResponse();
        
        response.Showtimes.AddRange(showtimes.ConvertAll(showtime => new ShowtimeResponse
        {
            Id = showtime.Id,
            SessionDate = showtime.SessionDate.ToString("o"),
            MovieTitle = showtime.Movie.Title
        }).ToList());

        return response;
    }
}