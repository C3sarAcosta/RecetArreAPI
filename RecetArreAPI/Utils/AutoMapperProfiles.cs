using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RecetArreAPI.DTOs.Identity;

namespace RecetArreAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CredencialesUsuario, IdentityUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
