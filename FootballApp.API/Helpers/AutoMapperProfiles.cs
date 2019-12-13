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
            CreateMap<User, UserToReturnDto>();
            CreateMap<GroupForCreationDto, Group>();
            // CreateMap<Group, GroupToReturnDto>();
                // .ForMember(dest => dest.Username, opt =>
                // {
                //     opt.MapFrom(src => src.User.Username);
                // });
        }
    }
}