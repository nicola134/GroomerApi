using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Exceptions;
using GroomerApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GroomerApi.Services
{
    public class UserService : IUserService
    {
        private readonly GroomerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(GroomerDbContext dbContext, IMapper mapper, ILogger<UserService> logger, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
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
                throw new NotFoundException("User not found");
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

            var hashePassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashePassword;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public void Delete(int id)
        {
            _logger.LogError($"User with id: {id} DELETE action invoked");

            var user = _dbContext
                .Users
                .FirstOrDefault(r => r.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

        }

        public void Update(UpdateUserDto dto, int id)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(r => r.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.PhoneNumber= dto.PhoneNumber;

            _dbContext.SaveChanges();

        }
    }
}
