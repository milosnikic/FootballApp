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
                .ForMember(
                    dest => dest.Age,
                    opt =>
                    {
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge());
                    })
                .ForMember(
                    dest => dest.MainPhoto,
                    opt => 
                    {
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain));
                    }
                );
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
            CreateMap<Photo, PhotoToReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<VisitUserDto, Visit>();
            CreateMap<Visit, VisitToReturnDto>()
                .ForMember(
                    dest => dest.Visitor,
                    opt => 
                    {
                        opt.MapFrom(src => src.Visitor);
                    }
                );
            CreateMap<CommentForCreationDto, Comment>()
                .ForMember(
                    dest => dest.Created,
                    opt => 
                    {
                        opt.MapFrom(src => src.DateCommented);
                    }
                );
            CreateMap<Comment, CommentToReturn>()
                .ForMember(
                    dest => dest.DateCommented,
                    opt => 
                    {
                        opt.MapFrom(src => src.Created);
                    }
                );
            CreateMap<GainedAchievement, GainedAchievementToReturnDto>()
                .ForMember(
                    dest => dest.Icon,
                    opt => 
                    {
                        opt.MapFrom(src => src.Achievement.Icon);
                    }
                )
                .ForMember(
                    dest => dest.Name,
                    opt => 
                    {
                        opt.MapFrom(src => src.Achievement.Name);
                    }
                );
        }
    }
}