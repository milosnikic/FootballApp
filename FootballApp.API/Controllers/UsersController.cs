using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data;
using FootballApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromRepo = await _usersRepository.GetUser(id);

            var userToReturn = _mapper.Map<UserToReturnDto>(userFromRepo);
            return Ok(userToReturn);
        }

    }
}