using System.Globalization;
using Application.Dtos;
using FluentValidation;

namespace Application.Validation;

public class BudgetDtoValidator : AbstractValidator<BudGetRequestDto>
{
	public BudgetDtoValidator()
	{
        RuleFor(p => p.PlanId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(model => model.ForecastDate)
            .NotEmpty().WithMessage("The field must be informed")
            .Must(BeAValidDate).WithMessage("ForecastDate must be a valid date in the format yyyy-MM-dd")
            .Must(BeGreaterThanOrEqualToToday).WithMessage("ForecastDate cannot be in the past")
            .Must(BeGreaterThanOrEqualToStartDateRent).WithMessage("ForecastDate should be from tomorrow onwards");
    }

    private bool BeAValidDate(DateTime date)
        => DateTime.TryParseExact(date.Date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    
    private bool BeGreaterThanOrEqualToToday(DateTime date)
    {
        var _date = date.Date.ToString("yyyy-MM-dd");

        if (DateTime.TryParseExact(_date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return parsedDate.Date >= DateTime.Today;
        }
        return false;
    }

    private bool BeGreaterThanOrEqualToStartDateRent(DateTime date)
    {
        var _date = date.Date.ToString("yyyy-MM-dd");

        if (DateTime.TryParseExact(_date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return parsedDate.Date >= DateTime.Today.AddDays(1);
        }
        return false;
    }
}
