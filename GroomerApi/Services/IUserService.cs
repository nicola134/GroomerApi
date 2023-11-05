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
        void Delete(int id);
        void Update(UpdateUserDto dto, int id);
    }
}
