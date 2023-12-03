using AutoMapper;
using GroomerApi.Authorization;
using GroomerApi.Entities;
using GroomerApi.Exceptions;
using GroomerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GroomerApi.Services
{
    public class UserService : IUserService
    {
        private readonly GroomerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public UserService(GroomerDbContext dbContext, IMapper mapper, ILogger<UserService> logger, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IAuthorizationService authorizationService
            , IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
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
        public IEnumerable<UserDto> GetAll(string searchPhrase)
        {
            var users = _dbContext
                .Users
                .Include(r => r.Address)
                .Include(r => r.Animals)
                .Where(r => searchPhrase == null ||  (r.FirstName.ToLower().Contains(searchPhrase.ToLower()) || r.LastName.ToLower().Contains(searchPhrase.ToLower())))
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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.PhoneNumber= dto.PhoneNumber;

            _dbContext.SaveChanges();

        }
        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(r => r.Role)
                .FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result =_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
