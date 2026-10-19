using Microsoft.AspNetCore.Mvc;
using USER_DEPS.Dto.Department;
using USER_DEPS.Entity;
using USER_DEPS.Repositories;
using USER_DEPS.Services;

namespace USER_DEPS.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _service.GetAllAsync();
            return Ok(departments);
        }

        // GET: api/Department/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _service.GetByIdAsync(id);

            if (department is null)
                return NotFound(new
                {
                    message = "Department tapılmadı."
                });

            return Ok(department);
        }

        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> Create(
            DepartmentCreateDto dto)
        {
            var department = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id },
                department);
        }

        // PUT: api/Department/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            DepartmentUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return NotFound(new
                {
                    message = "Department tapılmadı."
                });

            return NoContent();
        }

        // DELETE: api/Department/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new
                {
                    message = "Department tapılmadı."
                });

            return NoContent();
        }
    }
}