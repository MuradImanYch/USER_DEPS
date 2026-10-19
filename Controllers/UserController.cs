using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using USER_DEPS.Data;
using USER_DEPS.Dto.Department;
using USER_DEPS.Dto.User;
using USER_DEPS.Entity;
using USER_DEPS.Repositories;
using USER_DEPS.Services;

namespace USER_DEPS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);

            if (user is null)
                return NotFound(new { message = "User tapılmadı." });

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if (!result.Success)
                return Conflict(new { message = result.Message });

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Data!.Id },
                result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UserUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result.Success)
            {
                if (result.Message == "User tapılmadı.")
                    return NotFound(new { message = result.Message });

                if (result.Message == "PersonalId mövcud deyil.")
                    return BadRequest(new { message = result.Message });

                return Conflict(new { message = result.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "User tapılmadı." });

            return NoContent();
        }
    }
}