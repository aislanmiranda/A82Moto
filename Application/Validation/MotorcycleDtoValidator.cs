using Application.Dtos;
using FluentValidation;

namespace Application.Validation;

public class MotorcycleRequestDtoValidator : AbstractValidator<MotorcycleRequestDto>
{
    public MotorcycleRequestDtoValidator()
    {
        RuleFor(p => p.Year)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("The field must be greater than zero")
            .Must(HaveAtLeastFourDigits).WithMessage("The number must have at least 4 digits");

        RuleFor(p => p.Model)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(p => p.Plate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull().WithMessage("The field must be informed")
            .Matches(@"^[A-Za-z]{3}-\d{4}$")
            .WithMessage("The code must be in the format 'AAA-1234', with three letters followed by a hyphen and four digits");
    }

    private bool HaveAtLeastFourDigits(int number)
    {
        return number.ToString().Length >= 4;
    }
}

public class MotorcycleUpdatePlateDtoValidator : AbstractValidator<MotocycleUpdatePlateDto>
{
    public MotorcycleUpdatePlateDtoValidator()
    {
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(p => p.Plate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull().WithMessage("The field must be informed")
            .Matches(@"^[A-Za-z]{3}-\d{4}$")
            .WithMessage("The code must be in the format 'AAA-1234', with three letters followed by a hyphen and four digits");
    }
}