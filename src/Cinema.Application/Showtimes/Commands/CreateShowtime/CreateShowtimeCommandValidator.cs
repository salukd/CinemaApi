using FluentValidation;

namespace Cinema.Application.Showtimes.Commands.CreateShowtime;

public class CreateShowtimeCommandValidator : AbstractValidator<CreateShowtimeCommand>
{
    public CreateShowtimeCommandValidator()
    {
        RuleFor(r => r.AuditoriumId).GreaterThan(0);
        RuleFor(r => r.MovieId).GreaterThan(0);
        RuleFor(r => r.SessionDate.Date).GreaterThanOrEqualTo(DateTime.Now.Date);
    }
}