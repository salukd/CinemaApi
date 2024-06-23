using FluentValidation;

namespace Cinema.Application.Showtimes.Queries.GetFiltered;

public class GetFilteredShowtimeQueryValidator : AbstractValidator<GetFilteredShowtimeQuery>
{
    public GetFilteredShowtimeQueryValidator()
    {
        RuleFor(r => r.Query).NotEmpty();
    }
}