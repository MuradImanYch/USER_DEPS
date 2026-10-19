using Microsoft.EntityFrameworkCore;
using USER_DEPS.Data;
using USER_DEPS.Dto.User;
using USER_DEPS.Entity;
using USER_DEPS.Repositories;

namespace USER_DEPS.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly AppDbContext _context;

        public UserService(
            IRepository<User> repository,
            AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.Personal)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.Personal)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(bool Success, string Message, User? Data)>
            CreateAsync(UserCreateDto dto)
        {
            var personalExists = await _context.Personals
                .AnyAsync(x => x.Id == dto.PersonalId);

            if (!personalExists)
                return (false, "PersonalId mövcud deyil.", null);

            var usernameExists = await _context.Users
                .AnyAsync(x => x.Username == dto.Username);

            if (usernameExists)
                return (false, "Username artıq mövcuddur.", null);

            var personalHasUser = await _context.Users
                .AnyAsync(x => x.PersonalId == dto.PersonalId);

            if (personalHasUser)
                return (false, "Bu Personal üçün User artıq mövcuddur.", null);

            var user = new User
            {
                Username = dto.Username,
                PersonalId = dto.PersonalId
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return (true, "User uğurla yaradıldı.", user);
        }

        public async Task<(bool Success, string Message)>
            UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user is null)
                return (false, "User tapılmadı.");

            var personalExists = await _context.Personals
                .AnyAsync(x => x.Id == dto.PersonalId);

            if (!personalExists)
                return (false, "PersonalId mövcud deyil.");

            var usernameExists = await _context.Users
                .AnyAsync(x => x.Username == dto.Username && x.Id != id);

            if (usernameExists)
                return (false, "Username artıq istifadə olunur.");

            var personalHasOtherUser = await _context.Users
                .AnyAsync(x => x.PersonalId == dto.PersonalId && x.Id != id);

            if (personalHasOtherUser)
                return (false, "Bu Personal başqa User-ə bağlıdır.");

            user.Username = dto.Username;
            user.PersonalId = dto.PersonalId;

            _repository.Update(user);
            await _repository.SaveChangesAsync();

            return (true, "User uğurla yeniləndi.");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user is null)
                return false;

            _repository.Delete(user);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
