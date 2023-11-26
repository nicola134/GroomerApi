using GroomerApi.Models;

namespace GroomerApi.Services
{
    public interface IAnimalService
    {
        int Create(int userId, CreateAnimalDto dto);
        AnimalDto GetByID(int userId, int animalId);
        List<AnimalDto> GetAll(int userId);
        void DeleteAll(int userId);
        void Delete(int userId, int animalId);

    }
}
