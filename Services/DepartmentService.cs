using Microsoft.EntityFrameworkCore;
using USER_DEPS.Data;
using USER_DEPS.Dto.Department;
using USER_DEPS.Entity;
using USER_DEPS.Repositories;

namespace USER_DEPS.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        private readonly IRepository<Department> _repository;

        public DepartmentService(
            AppDbContext context,
            IRepository<Department> repository)
        {
            _context = context;
            _repository = repository;
        }


        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Personals)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Department> CreateAsync(DepartmentCreateDto dto)
        {
            var department = new Department
            {
                Name = dto.Name
            };

            await _repository.AddAsync(department);
            await _repository.SaveChangesAsync();

            return department;
        }

        public async Task<bool> UpdateAsync(
            int id,
            DepartmentUpdateDto dto)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department is null)
                return false;

            department.Name = dto.Name;

            _repository.Update(department);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department is null)
                return false;

            _repository.Delete(department);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
