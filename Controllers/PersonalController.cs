using Microsoft.AspNetCore.Mvc;
using USER_DEPS.Dto.Personal;
using USER_DEPS.Entity;
using USER_DEPS.Repositories;
using USER_DEPS.Services;

namespace USER_DEPS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalController : ControllerBase
    {
        private readonly IPersonalService _service;

        public PersonalController(IPersonalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var personal = await _service.GetByIdAsync(id);

            if (personal is null)
                return NotFound(new { message = "Personal tapılmadı." });

            return Ok(personal);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonalCreateDto dto)
        {
            var personal = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = personal.Id },
                personal);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            PersonalUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return NotFound(new { message = "Personal tapılmadı." });

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "Personal tapılmadı." });

            return NoContent();
        }
    }
}