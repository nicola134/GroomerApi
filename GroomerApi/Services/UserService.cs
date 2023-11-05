using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GroomerApi.Services
{
    public class UserService : IUserService
    {
        private readonly GroomerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        public UserService(GroomerDbContext dbContext, IMapper mapper, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public UserDto GetById(int id)
        {
            var user = _dbContext
                .Users
                .Include(r => r.Address)
                .Include(r => r.Animals)
                .FirstOrDefault(r => r.Id == id);
            if (user == null)
            {
                return null;
            }

            var result = _mapper.Map<UserDto>(user);
            return result;
        }
        public IEnumerable<UserDto> GetAll() 
        {
            var users = _dbContext
                .Users 
                .Include(r => r.Address)
                .Include(r => r.Animals)
                .ToList();

            var usersDto = _mapper.Map<List<UserDto>>(users);
            return usersDto;
        }

        public int Create(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public bool Delete(int id)
        {
            _logger.LogError($"User with id: {id} DELETE action invoked");

            var user = _dbContext
                .Users
                .FirstOrDefault(r => r.Id == id);

            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return true;
        }

        public bool Update(UpdateUserDto dto, int id)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(r => r.Id == id);

            if (user == null)
            {
                return false;
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.PhoneNumber= dto.PhoneNumber;

            _dbContext.SaveChanges();

            return true;

        }
    }
}
