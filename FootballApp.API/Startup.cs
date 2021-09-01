using System.Text;
using AutoMapper;
using FootballApp.API.Data;
using FootballApp.API.Data.Groups;
using FootballApp.API.Data.Photos;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Data.Users;
using FootballApp.API.Helpers;
using FootballApp.API.Hubs;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FootballApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddCors();
            services.AddMvc()
                .AddJsonOptions(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            RegisterRepositories(services);
            RegisterServices(services);
            services.AddTransient<DataSeed>();
            services.AddAutoMapper();
            services.AddSwaggerDocumentation();
            services.AddSignalR();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.
                            GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext context, DataSeed seed)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.Database.Migrate();
                seed.SeedDatabase();
            }
            else
            {
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerDocumentation();
            app.UseAuthentication();
            app.UseCors(builder => builder.WithOrigins("http://localhost:4200")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials()
                                            .SetIsOriginAllowed((host) => true));
            app.UseMvc();
            app.UseSignalR(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
