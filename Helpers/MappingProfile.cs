using AutoMapper;
using DattingApp.Dto;
using DattingApp.Entities;
using DattingApp.Extensions;
using System.Linq;

namespace DattingApp.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(destination => destination.Age, memberOption => memberOption.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
