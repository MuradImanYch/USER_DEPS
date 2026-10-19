using USER_DEPS.Dto.Department;
using USER_DEPS.Entity;

namespace USER_DEPS.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department> CreateAsync(DepartmentCreateDto dto);

        Task<bool> UpdateAsync(int id, DepartmentUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
