using Application.Dtos;
using Application.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class rentController : Controller
{
    private readonly IRentService _service;

    public rentController(IRentService service)
        => _service = service;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RentRequestDto request)
    {
        await _service.AddAsync(request);

        return Created("", request);
    }

    [HttpPost("budget")]
    public async Task<IActionResult> GetBudGet([FromBody]BudGetRequestDto request,
        [FromServices]IValidator<BudGetRequestDto> validator)
    {
        var result = validator.Validate(request);
        if (result.Errors.Count > 0)
            throw new ValidationException("Error", result.Errors);

        var budget = await _service.ConsultBudgetAsync(request);
        return Ok(budget);
    }

    [HttpGet("rentals")]
    public async Task<IActionResult> Rentals()
    {
        var rentails = await _service.GetAllAsync();

        return Ok(rentails);
    }
}