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
                )
                .ForMember(
                    dest => dest.Flag,
                    opt =>
                    {
                        opt.MapFrom(src => src.Country.Flag);
                    }
                );
            CreateMap<User, UserToReturnMiniDto>()
                .ForMember(
                    dest => dest.MainPhoto,
                    opt =>
                    {
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Image);
                    }
                );
            CreateMap<ChatUser, UserToReturnMiniDto>()
                .ForMember(
                    dest => dest.Id,
                    opt =>
                    {
                        opt.MapFrom(src => src.UserId);
                    })
                .ForMember(
                    dest => dest.Firstname,
                    opt =>
                    {
                        opt.MapFrom(src => src.User.Firstname);
                    })
                .ForMember(
                    dest => dest.Lastname,
                    opt =>
                    {
                        opt.MapFrom(src => src.User.Lastname);
                    })
                .ForMember(
                    dest => dest.Username,
                    opt =>
                    {
                        opt.MapFrom(src => src.User.Username);
                    })
                .ForMember(
                    dest => dest.Gender,
                    opt =>
                    {
                        opt.MapFrom(src => src.User.Gender);
                    })
                .ForMember(
                    dest => dest.MainPhoto,
                    opt =>
                    {
                        opt.MapFrom(src => src.User.Photos.FirstOrDefault(p => p.IsMain).Image);
                    });


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
            CreateMap<User, UserForDisplayDto>()
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
                    dest => dest.Flag,
                    opt =>
                    {
                        opt.MapFrom(src => src.Country.Flag);
                    }
                )
                .ForMember(
                    dest => dest.MatchStatus,
                    opt =>
                    {
                        opt.MapFrom(src => src.MatchStatuses.SingleOrDefault());
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
            CreateMap<MatchdayForCreationDto, Matchday>()
                .ForMember(
                    dest => dest.Group,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.Location,
                    opt => opt.Ignore()
                );
            CreateMap<Membership, MembershipInformationDto>();
            CreateMap<Membership, DetailGroupToReturnDto>()
                .ForMember(
                    dest => dest.Image,
                    opt =>
                    {
                        opt.MapFrom(src => src.Group.Image);
                    }
                )
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
                    dest => dest.NumberOfMembers,
                    opt =>
                    {
                        opt.MapFrom(src => src.Group.Memberships
                        .Where(m => m.MembershipStatus == MembershipStatus.Accepted)
                        .ToArray()
                        .Length);
                    }
                )
                .ForMember(
                    dest => dest.LatestJoined,
                    opt =>
                    {
                        opt.MapFrom(src => src.Group.Memberships
                                    .Where(m => m.MembershipStatus == MembershipStatus.Accepted)
                                    .OrderByDescending(m => m.DateAccepted)
                                    .Take(10)
                                    .Select(m => m.User)
                                    .ToList());
                    }
                )
                .ForMember(
                    dest => dest.PendingRequests,
                    opt =>
                    {
                        opt.MapFrom(src => src.Group.Memberships
                                    .Where(m => m.MembershipStatus == MembershipStatus.Sent)
                                    .Select(m => m.User)
                                    .ToList());
                    }
                )
                .ForMember(
                    dest => dest.Members,
                    opt =>
                    {
                        opt.MapFrom(src => src.Group.Memberships.Where(m => m.MembershipStatus == MembershipStatus.Accepted).Select(m => m.User));
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
                );
            CreateMap<MatchStatus, MatchdayForDisplayDto>()
                .ForMember(
                    dest => dest.Id,
                    opt =>
                    {
                        opt.MapFrom(src => src.MatchdayId);
                    }
                )
                .ForMember(
                    dest => dest.Name,
                    opt =>
                    {
                        opt.MapFrom(src => src.Matchday.Name);
                    }
                )
                .ForMember(
                    dest => dest.Location,
                    opt =>
                    {
                        opt.MapFrom(src => src.Matchday.Location.Name);
                    }
                )
                .ForMember(
                    dest => dest.City,
                    opt =>
                    {
                        opt.MapFrom(src => src.Matchday.Location.City.Name);
                    }
                )
                .ForMember(
                    dest => dest.Country,
                    opt =>
                    {
                        opt.MapFrom(src => src.Matchday.Location.Country.Name);
                    }
                )
                .ForMember(
                    dest => dest.NumberOfPlayers,
                    opt =>
                    {
                        opt.MapFrom(src => src.Matchday.NumberOfPlayers);
                    }
                )
                .ForMember(
                    dest => dest.DatePlaying,
                    opt =>
                    {
                        opt.MapFrom(src => src.Matchday.DatePlaying);
                    }
                )
                .ForMember(
                    dest => dest.NumberOfConfirmedPlayers,
                    opt =>
                    {
                        opt.MapFrom(src => src.Matchday.MatchStatuses.Where(m => m.Confirmed == true).ToArray().Length);
                    }
                );
            CreateMap<Matchday, MatchdayToReturnDto>()
                .ForMember(
                    dest => dest.Location,
                    opt =>
                    {
                        opt.MapFrom(src => src.Location.Name);
                    }
                )
                .ForMember(
                    dest => dest.City,
                    opt =>
                    {
                        opt.MapFrom(src => src.Location.City.Name);
                    }
                )
                .ForMember(
                    dest => dest.Country,
                    opt =>
                    {
                        opt.MapFrom(src => src.Location.Country.Name);
                    }
                )
                .ForMember(
                    dest => dest.NumberOfConfirmedPlayers,
                    opt =>
                    {
                        opt.MapFrom(src => src.MatchStatuses.Where(m => m.Confirmed == true).ToArray().Length);
                    }
                )
                .ForMember(
                    dest => dest.AppliedUsers,
                    opt =>
                    {
                        opt.MapFrom(src => src.MatchStatuses.Select(m => m.User));
                    }
                )
                .ForMember(
                    dest => dest.GroupId,
                    opt => 
                    {
                        opt.MapFrom(src => src.Group.Id);
                    }
                );

            CreateMap<MessageToSendDto, Message>()
                .ForMember(
                    dest => dest.Sender,
                    opt => opt.Ignore()
                );

            CreateMap<Message, MessageToReturnDto>();

            CreateMap<ChatToReturnDto, Chat>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.Ignore()
                );

            CreateMap<Chat, ChatToReturnDto>()
                .ForMember(
                    dest => dest.LastMessage,
                    opt =>
                    {
                        opt.MapFrom(src => src.Messages.LastOrDefault().MessageSent);
                    }
                );

        }
    }
}