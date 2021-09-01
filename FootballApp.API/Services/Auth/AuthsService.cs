using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FootballApp.API.Services.Auth
{
    public class AuthsService : IAuthsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthsService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        public async Task<LoginUserToReturnDto> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _unitOfWork.Auths.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return null;

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

            return new LoginUserToReturnDto 
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
        }

        public async Task<UserToReturnDto> Register(UserForRegisterDto userForRegisterDto)
        {
            var city = await _unitOfWork.Cities.GetById(userForRegisterDto.City);
            var country = await _unitOfWork.Countries.GetById(userForRegisterDto.Country);
            if (city == null || country == null)
            {
                return null;
            }

            var userToCreate = _mapper.Map<CommonUser>(userForRegisterDto);
            userToCreate.Country = country;
            userToCreate.City = city;

            var createdUser = await _unitOfWork.Auths.Register(userToCreate, userForRegisterDto.Password);
            if (!await _unitOfWork.Complete())
                return null;


            var userToReturn = _mapper.Map<UserToReturnDto>(createdUser);
            return userToReturn;
        }
    }
}