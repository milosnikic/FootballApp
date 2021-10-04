using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthsService _authsService;
        public AuthController(IAuthsService authsService)
        {
            _authsService = authsService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToReturn = await _authsService.Register(userForRegisterDto);
            if (userToReturn != null)
                return Ok(userToReturn);

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            return Ok(await _authsService.Login(userForLoginDto));
        }
    }
}