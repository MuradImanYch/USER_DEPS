using USER_DEPS.Dto.Personal;
using USER_DEPS.Entity;
using USER_DEPS.Repositories;

namespace USER_DEPS.Services
{
    public class PersonalService : IPersonalService
    {
        private readonly IRepository<Personal> _repository;

        public PersonalService(IRepository<Personal> repository)
        {
            _repository = repository;
        }

        public async Task<List<Personal>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Personal?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Personal> CreateAsync(PersonalCreateDto dto)
        {
            var personal = new Personal
            {
                Name = dto.Name,
                Surname = dto.Surname,
                FatherName = dto.FatherName
            };

            await _repository.AddAsync(personal);
            await _repository.SaveChangesAsync();

            return personal;
        }

        public async Task<bool> UpdateAsync(
            int id,
            PersonalUpdateDto dto)
        {
            var personal = await _repository.GetByIdAsync(id);

            if (personal is null)
                return false;

            personal.Name = dto.Name;
            personal.Surname = dto.Surname;
            personal.FatherName = dto.FatherName;

            _repository.Update(personal);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var personal = await _repository.GetByIdAsync(id);

            if (personal is null)
                return false;

            _repository.Delete(personal);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
