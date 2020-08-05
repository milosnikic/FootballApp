using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FootballApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _unitOfWork.Auths.UserExists(userForRegisterDto.Username))
                return BadRequest("User with specified username already exists");

            var city = await _unitOfWork.Cities.GetById(userForRegisterDto.City);
            var country = await _unitOfWork.Countries.GetById(userForRegisterDto.Country);
            if(city == null || country == null)
            {
                return BadRequest("Invalid city or country specified");
            }
            
            var userToCreate = _mapper.Map<CommonUser>(userForRegisterDto);
            userToCreate.Country = country;
            userToCreate.City = city;

            var createdUser = await _unitOfWork.Auths.Register(userToCreate, userForRegisterDto.Password);
            if(!await _unitOfWork.Complete())
                return BadRequest("User couldn't be created.");                
            

            var userToReturn = _mapper.Map<UserToReturnDto>(createdUser);
            return Ok(new
            {
                userToReturn
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _unitOfWork.Auths.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return BadRequest("Not valid credentials.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserToReturnDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}