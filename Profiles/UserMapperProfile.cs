using AutoMapper;
using DatingApp.API.Data.Entities;
using DatingApp.API.DTOs;
using DatingApp.API.Extensions;

namespace DatingApp.API.Profiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, MemberDTO>()
            .ForMember(dest => dest.Age,
            options => options.MapFrom(src => (src.DateOfBirth.HasValue? 
            src.DateOfBirth.Value.CalculateAge() : 0)));

        }
    }
}