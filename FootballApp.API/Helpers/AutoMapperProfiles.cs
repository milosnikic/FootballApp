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
            CreateMap<UserForRegisterDto, CommonUser>()
                .ForMember(
                    dest => dest.City,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.Country,
                    opt => opt.Ignore()
                );
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
                )
                .ForMember(
                    dest => dest.City,
                    opt => 
                    {
                        opt.MapFrom(src => src.City.Name);
                    }
                )
                .ForMember(
                    dest => dest.Country,
                    opt => 
                    {
                        opt.MapFrom(src => src.Country.Name);
                    }
                )
                .ForMember(
                    dest => dest.IsPowerUser,
                    opt => 
                    {
                        opt.MapFrom(src => src is PowerUser);
                    }
                )
                .ForMember(
                    dest => dest.NumberOfGroupsCreated,
                    opt =>
                    {
                        opt.MapFrom(src => src is PowerUser ? (src as PowerUser).NumberOfGroupsCreated : -1);
                    }
                )
                .ForMember(
                    dest => dest.GroupsCreated,
                    opt => 
                    {
                        opt.MapFrom(src => src is PowerUser ? (src as PowerUser).GroupsCreated : null);
                    }
                );
            CreateMap<GroupForCreationDto, Group>()
                .ForMember(
                    dest => dest.Location,
                    opt => opt.Ignore()
                );
            CreateMap<Group, GroupToReturnDto>()
                .ForMember(
                    dest => dest.NumberOfMembers,
                    opt => 
                    {
                        opt.MapFrom(src => src.Memberships.Where(m => m.MembershipStatus == MembershipStatus.Accepted).ToArray().Length);
                    }
                );
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
                )
                .ForMember(
                    dest => dest.Value,
                    opt => 
                    {
                        opt.MapFrom(src => src.Achievement.Value);
                    }
                );
            CreateMap<User, ExploreUserDto>()
                .ForMember(
                    dest => dest.MainPhoto,
                    opt => 
                    {
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Image);
                    }
                );
            CreateMap<CityForCreationDto, City>();
            CreateMap<LocationToAddDto, Location>();
            CreateMap<Location, LocationToReturnDto>()
                .ForMember(
                    dest => dest.Country,
                    opt => 
                    {
                        opt.MapFrom(src => src.Country.Name);
                    }
                )
                .ForMember(
                    dest => dest.City,
                    opt => 
                    {
                        opt.MapFrom(src => src.City.Name);
                    }
                );
            CreateMap<Country, CountryToReturnDto>();
            CreateMap<Membership, GroupToReturnDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.Id);
                    }
                )
                .ForMember(
                    dest => dest.Name,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.Name);
                    }
                )
                .ForMember(
                    dest => dest.Description,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.Description);
                    }
                )
                .ForMember(
                    dest => dest.DateCreated,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.DateCreated);
                    }
                )
                .ForMember(
                    dest => dest.NumberOfMembers,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.Memberships.Where(m => m.MembershipStatus == MembershipStatus.Accepted).ToArray().Length);
                    }
                )
                .ForMember(
                    dest => dest.Location,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.Location);
                    }
                )
                .ForMember(
                    dest => dest.Image,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.Image);
                    }
                );
        }
    }
}