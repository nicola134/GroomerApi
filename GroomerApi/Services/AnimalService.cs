using AutoMapper;
using DevExpress.DirectX.Common.Direct2D;
using GroomerApi.Entities;
using GroomerApi.Exceptions;
using GroomerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GroomerApi.Services
{
    public class AnimalService : IAnimalService
    {
        private GroomerDbContext _context;
        private IMapper _mapper;

        public AnimalService(GroomerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int Create(int userId, CreateAnimalDto dto)
        {
            var user = GetUserById(userId);

            var animalEntity = _mapper.Map<Animal>(dto);

            animalEntity.UserId = userId;

            _context.Animals.Add(animalEntity);
            _context.SaveChanges();

            return animalEntity.Id;
        }

        public AnimalDto GetByID(int userId, int animalId)
        {
            var user = GetUserById(userId);

            var animal = GetAnimalById(userId, animalId);
            var animalDto = _mapper.Map<AnimalDto>(animal);
            return animalDto;
        }

        public List<AnimalDto> GetAll(int userId)
        {
            var user = GetUserById(userId);

            var animals = user.Animals;
            var animalsDto = _mapper.Map<List<AnimalDto>>(animals);

            return animalsDto;
        }

        public void DeleteAll(int userId)
        {
            var user = GetUserById(userId);

            _context.RemoveRange(user.Animals);
            _context.SaveChanges();
        }

        public void Delete(int userId, int animalId)
        {
            var user = GetUserById(userId);

            var animal = GetAnimalById(userId, animalId);

            _context.Remove(animal);
            _context.SaveChanges();
        }

        private Animal GetAnimalById(int userId, int animalId)
        {
            var animal = _context.Animals.FirstOrDefault(a => a.Id == animalId);
            if (animal is null || animal.UserId != userId)
            {
                throw new NotFoundException("Animal not found");
            }
            return animal;
        }
        private User GetUserById(int userId)
        {
            var user = _context
                .Users
                .Include(u => u.Animals)
                .FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }
    }
}
