using Application.Dtos;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class authController : Controller
{
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]AuthRequestDto request)
    {

        return Ok(request);
    }
}