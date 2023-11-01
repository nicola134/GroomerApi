using AutoMapper;
using GroomerApi.Entities;
using GroomerApi.Models;

namespace GroomerApi
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Animal, AnimalDto>();

            CreateMap<CreateUserDto, User>()
                .ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
                {
                    City = dto.City,
                    PostalCode = dto.PostalCode,
                    Street = dto.Street,
                }));
        }
    }
}

