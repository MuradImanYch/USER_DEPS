using USER_DEPS.Dto.Personal;
using USER_DEPS.Entity;

namespace USER_DEPS.Services
{
    public interface IPersonalService
    {
        Task<List<Personal>> GetAllAsync();

        Task<Personal?> GetByIdAsync(int id);

        Task<Personal> CreateAsync(PersonalCreateDto dto);

        Task<bool> UpdateAsync(int id, PersonalUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
