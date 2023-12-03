using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;
using System.Security.Claims;

namespace GroomerApi.Services
{
    public interface IUserService
    {
        PagedResult<UserDto> GetAll(UserQuery query);
        UserDto GetById(int id);
        int Create(CreateUserDto dto);
        void Delete(int id);
        void Update(UpdateUserDto dto, int id);
        string GenerateJwt(LoginDto dto);
    }
}
