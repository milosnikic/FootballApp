using System.Linq;
using AutoMapper;
using FootballApp.API.Dtos;
using FootballApp.API.Models;

namespace FootballApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserToReturnDto>()
                .ForMember(dest => dest.Age,
                    opt => {
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                    });
            CreateMap<GroupForCreationDto, Group>();
            CreateMap<Group, GroupToReturnDto>()
                .ForMember(
                 dest => dest.UserId,
                 opt =>
                {
                    opt.MapFrom(src => src.Memberships.Where(m => m.GroupId == src.Id).FirstOrDefault().User.Id);
                })
                .ForMember(
                 dest => dest.Username,
                 opt =>
                {
                    opt.MapFrom(src => src.Memberships.Where(m => m.GroupId == src.Id).FirstOrDefault().User.Username);
                });
        }
    }
}