using Application.Dtos;
using Application.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class deliverymanController : Controller
{
    private readonly IDeliveryManService _service;

    public deliverymanController(IDeliveryManService service)
        => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _service.GetAll();
        return Ok(results);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]DeliveryManRequestDto request,
        [FromServices]IValidator<DeliveryManRequestDto> validator)
    {
        var result = validator.Validate(request);
        if (result.Errors.Count > 0)
            throw new ValidationException("Error", result.Errors);

        await _service.AddAsync(request);

        return Created("", null);
    }

    [HttpPatch("photo")]
    public async Task<IActionResult> Patch([FromForm] DeliveryManUpdatePhotoDto request,
        [FromServices] IValidator<DeliveryManUpdatePhotoDto> validator)
    {
        var result = validator.Validate(request);
        if (result.Errors.Count > 0)
            throw new ValidationException("Error", result.Errors);

        await _service.UpdatePhotoAsync(request);

        return NoContent();
    }
}