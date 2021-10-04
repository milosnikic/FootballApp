using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Auth
{
    public interface IAuthsService
    {
        Task<UserToReturnDto> Register(UserForRegisterDto userForRegisterDto);
        Task<LoginUserToReturnDto> Login(UserForLoginDto userForLoginDto);
    }
}