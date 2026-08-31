using ConferenceBooking.Application.DTO;
using FluentValidation;

namespace ConferenceBooking.Application.Validators;

public class CreateHallDtoValidator : AbstractValidator<CreateHallDto>
{
    public CreateHallDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hall name is required")
            .MaximumLength(100);

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be positive");

        RuleFor(x => x.BasePricePerHour)
            .GreaterThan(0).WithMessage("Base price must be positive");

        RuleForEach(x => x.Services).SetValidator(new CreateServiceDtoValidator());
    }
}

public class UpdateHallDtoValidator : AbstractValidator<UpdateHallDto>
{
    public UpdateHallDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Capacity).GreaterThan(0);
        RuleFor(x => x.BasePricePerHour).GreaterThan(0);
    }
}

public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}