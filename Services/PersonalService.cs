
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using USER_DEPS.Data;
using USER_DEPS.Dto.Personal;
using USER_DEPS.Entity;
using USER_DEPS.Repositories;

namespace USER_DEPS.Services
{
    public class PersonalService : IPersonalService
    {
        private readonly IRepository<Personal> _repository;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonalService(
            IRepository<Personal> repository,
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<int?> GetCurrentDepartmentIdAsync()
        {
            var userIdValue = _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
                return null;

            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Personal == null
                    ? (int?)null
                    : u.Personal.DepartmentId)
                .FirstOrDefaultAsync();
        }

        // Yalnız login olan istifadəçinin departamentindəki işçilər
        public async Task<List<Personal>> GetAllAsync()
        {
            var departmentId = await GetCurrentDepartmentIdAsync();

            if (departmentId == null)
                return new List<Personal>();

            return await _context.Personals
                .AsNoTracking()
                .Where(p => p.DepartmentId == departmentId.Value)
                .ToListAsync();
        }

        public async Task<Personal?> GetByIdAsync(int id)
        {
            var departmentId = await GetCurrentDepartmentIdAsync();

            if (departmentId == null)
                return null;

            return await _context.Personals
                .AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.DepartmentId == departmentId.Value);
        }

        // Yeni işçi login olan istifadəçinin departamentinə əlavə olunur
        public async Task<Personal> CreateAsync(PersonalCreateDto dto)
        {
            var departmentId = await GetCurrentDepartmentIdAsync();

            if (departmentId == null)
                throw new InvalidOperationException(
                    "İstifadəçinin departamenti müəyyən edilmədi.");

            var personal = new Personal
            {
                Name = dto.Name,
                Surname = dto.Surname,
                FatherName = dto.FatherName,
                DepartmentId = departmentId.Value
            };

            await _repository.AddAsync(personal);
            await _repository.SaveChangesAsync();

            return personal;
        }

        public async Task<bool> UpdateAsync(
            int id,
            PersonalUpdateDto dto)
        {
            var departmentId = await GetCurrentDepartmentIdAsync();

            if (departmentId == null)
                return false;

            var personal = await _context.Personals
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.DepartmentId == departmentId.Value);

            if (personal == null)
                return false;

            personal.Name = dto.Name;
            personal.Surname = dto.Surname;
            personal.FatherName = dto.FatherName;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var departmentId = await GetCurrentDepartmentIdAsync();

            if (departmentId == null)
                return false;

            var personal = await _context.Personals
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.DepartmentId == departmentId.Value);

            if (personal == null)
                return false;

            var hasUser = await _context.Users
                .AnyAsync(u => u.PersonalId == id);

            if (hasUser)
            {
                throw new InvalidOperationException(
                    "Bu işçiyə bağlı istifadəçi var. Əvvəlcə əlaqəni həll edin.");
            }

            _context.Personals.Remove(personal);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
