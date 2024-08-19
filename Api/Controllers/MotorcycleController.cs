using Application.Dtos;
using Application.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class motorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _service;

        public motorcycleController(IMotorcycleService service)
            => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll(bool inUse)
        {
            var results = await _service.GetAllAsync(inUse);
            return Ok(results);
        }

        [HttpGet("{plate}")]
        public async Task<IActionResult> GetByPlate(string plate)
        {
            var result = await _service.GetByPlateAsync(plate);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MotorcycleRequestDto request
            , [FromServices]IValidator<MotorcycleRequestDto> validator)
        {
                var result = validator.Validate(request);
                if (result.Errors.Count > 0)
                    throw new ValidationException("Error", result.Errors);

                await _service.AddAsync(request);

                return Created("", null);
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]MotocycleUpdatePlateDto request
            ,[FromServices] IValidator<MotocycleUpdatePlateDto> validator)
        {
            var result = validator.Validate(request);
            if (result.Errors.Count > 0)
                throw new ValidationException("Error", result.Errors);

            await _service.UpdatePlateAsync(request);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
    }
}

