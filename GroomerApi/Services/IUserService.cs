using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;
using System.Security.Claims;

namespace GroomerApi.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll(string searchPhrase);
        UserDto GetById(int id);
        int Create(CreateUserDto dto);
        void Delete(int id);
        void Update(UpdateUserDto dto, int id);
        string GenerateJwt(LoginDto dto);
    }
}
