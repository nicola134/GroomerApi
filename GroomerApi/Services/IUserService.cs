using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;

namespace GroomerApi.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        int Create(CreateUserDto dto);
        bool Delete(int id);
        bool Update(UpdateUserDto dto, int id);
    }
}
