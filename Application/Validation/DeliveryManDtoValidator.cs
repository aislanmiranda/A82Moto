using System.Linq;
using Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validation;

public class DeliveryManDtoValidator : AbstractValidator<DeliveryManRequestDto>
{
    public DeliveryManDtoValidator()
    {
        RuleFor(p => p.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(p => p.Cnpj)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(p => p.BirthDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(p => p.NumberCnh)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(model => model.TypeCnh)
           .NotEmpty().WithMessage("TypeCnh is required.")
           .Must(value => value == "A" || value == "B" || value == "AB")
           .WithMessage("TypeCnh must be one of the following values: 'A', 'B', or 'AB'.");
    }
}

public class DeliveryManUpdatePhotoDtoValidator : AbstractValidator<DeliveryManUpdatePhotoDto>
{
    public DeliveryManUpdatePhotoDtoValidator()
    {
        RuleFor(p => p.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .NotNull()
            .WithMessage("The field must be informed");

        RuleFor(model => model.PhotoFile)
               .NotNull().WithMessage("PhotoFile is required");
    }

    //private bool HaveValidExtension(IFormFile file)
    //{
    //    if (file == null)
    //        return false;

    //    var allowedExtensions = new[] { ".png", ".bmp" };
    //    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

    //    return allowedExtensions.Contains(extension);
    //}
}