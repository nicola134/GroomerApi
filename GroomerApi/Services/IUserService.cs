using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;
using System.Security.Claims;

namespace GroomerApi.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        int Create(CreateUserDto dto);
        void Delete(int id, ClaimsPrincipal claimsPrincipal);
        void Update(UpdateUserDto dto, int id, ClaimsPrincipal claimsPrincipal);
        string GenerateJwt(LoginDto dto);
    }
}
