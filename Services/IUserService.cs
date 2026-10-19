using USER_DEPS.Dto.User;
using USER_DEPS.Entity;

namespace USER_DEPS.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<(bool Success, string Message, User? Data)>
            CreateAsync(UserCreateDto dto);

        Task<(bool Success, string Message)>
            UpdateAsync(int id, UserUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
