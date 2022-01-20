using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Data;
using FootballApp.API.Data.Groups;
using FootballApp.API.Data.Photos;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Data.Users;
using FootballApp.API.Services;
using FootballApp.API.Services.Auth;
using FootballApp.API.Services.Chat;
using FootballApp.API.Services.Comments;
using FootballApp.API.Services.Friends;
using FootballApp.API.Services.Groups;
using FootballApp.API.Services.Locations;
using FootballApp.API.Services.Matches;
using FootballApp.API.Services.Photos;
using FootballApp.API.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace FootballApp.API.Helpers
{
    public static class Registrator
    {
        public static void Register(IServiceCollection services) 
        {
            Console.WriteLine("Registrating services...");
            RegisterServices(services);
            Console.WriteLine("Registrating repositories...");
            RegisterRepositories(services);

        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IPhotosService, PhotosService>();
            services.AddScoped<IMatchesService, MatchesService>();
            services.AddScoped<ILocationsService, LocationsService>();
            services.AddScoped<IGroupsService, GroupsService>();
            services.AddScoped<IFriendsService, FriendsService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IChatsService, ChatsService>();
            services.AddScoped<IAuthsService, AuthsService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IGroupsRepository, GroupsRepository>();
            services.AddScoped<IPhotosRepository, PhotosRepository>();
        }
    }
}