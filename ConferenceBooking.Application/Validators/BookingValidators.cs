using ConferenceBooking.Application.DTO;
using FluentValidation;

namespace ConferenceBooking.Application.Validators;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator()
    {
        RuleFor(x => x.HallId).GreaterThan(0);

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("EndTime must be after StartTime");
    }
}

public class SearchHallsDtoValidator : AbstractValidator<SearchHallsDto>
{
    public SearchHallsDtoValidator()
    {
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("EndTime must be after StartTime");

        RuleFor(x => x.MinCapacity)
            .GreaterThanOrEqualTo(0);
    }
}