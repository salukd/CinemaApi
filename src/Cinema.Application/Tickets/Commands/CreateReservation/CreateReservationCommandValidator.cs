using FluentValidation;

namespace Cinema.Application.Tickets.Commands.CreateReservation;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(r => r.ShowtimeId)
            .GreaterThan(0)
            .WithMessage("Showtime ID must be greater than 0.");

        RuleFor(r => r.Row).GreaterThan((short)0);
        
        RuleFor(r => r.Seats)
            .NotEmpty()
            .WithMessage("Seat numbers cannot be empty.")
            .Must(BeContiguous)
            .WithMessage("All seats must be contiguous.");
    }

    private bool BeContiguous(List<int> seats)
    {
        seats.Sort();
        
        for (var i = 1; i < seats.Count; i++)
        {
            if (seats[i] != seats[i - 1] + 1)
            {
                return false;
            }
        }

        return true;
    }
}