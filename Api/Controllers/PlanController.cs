using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class planController : Controller
{
    private readonly IPlanService _service;

    public planController(IPlanService service)
        => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _service.GetAllAsync();
        return Ok(results);
    }
}