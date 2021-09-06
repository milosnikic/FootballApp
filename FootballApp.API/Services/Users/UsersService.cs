using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Services.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballApp.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UsersService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<KeyValuePair<bool, string>> GainAchievement(int userId, GainedAchievementForCreationDto gainedAchievementForCreationDto)
        {
            var achievement = await _unitOfWork.Achievements.GetAchievementByValue(gainedAchievementForCreationDto.Value);

            if (achievement == null)
            {
                return new KeyValuePair<bool, string>(false, "Not valid achievement!");
            }

            var gainedAchievement = await _unitOfWork.Achievements.GetGainedAchievement(userId, gainedAchievementForCreationDto.Value);
            if (gainedAchievement != null)
            {
                return new KeyValuePair<bool, string>(false, "Already have gained that achievement!");
            }

            gainedAchievement = new GainedAchievement
            {
                UserId = userId,
                User = await _unitOfWork.Users.GetById(userId),
                DateAchieved = DateTime.Now,
                Achievement = achievement,
                AchievementId = achievement.Id
            };

            _unitOfWork.Users.GainAchievement(gainedAchievement);
            if (await _unitOfWork.Complete())
                return new KeyValuePair<bool, string>(true, "Achievement successfully gained!");

            return new KeyValuePair<bool, string>(false, "Problem gaining achievement!");
        }

        public async Task<ICollection<Achievement>> GetAllAchievements()
        {
            return await _unitOfWork.Achievements.GetAll();
        }

        public async Task<ICollection<GainedAchievementToReturnDto>> GetAllAchievementsForUser(int userId)
        {
            var achievements = await _unitOfWork.Users
                                          .GetAllAchievementsForUser(userId);

            return _mapper.Map<ICollection<GainedAchievementToReturnDto>>(achievements);
        }

        public async Task<ICollection<UserToReturnDto>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetUsers();

            return _mapper.Map<ICollection<UserToReturnDto>>(users);
        }

        public async Task<ICollection<VisitToReturnDto>> GetLatestFiveVisitorsForUser(int userId)
        {
            var visitors = await _unitOfWork.Users.GetLatestFiveVisitorsForUser(userId);

            return _mapper.Map<ICollection<VisitToReturnDto>>(visitors);
        }

        public async Task<UserToReturnDto> GetUser(int id)
        {
            var userFromRepo = await _unitOfWork.Users.GetUserByIdWithAdditionalInformation(id);

            if (userFromRepo == null)
            {
                return null;
            }

            var userToReturn = _mapper.Map<UserToReturnDto>(userFromRepo);
            return userToReturn;
        }

        public async Task<KeyValuePair<bool, string>> UpdateUser(int userId, UserToUpdateDto userToUpdateDto)
        {
            var user = await _unitOfWork.Users.GetById(userId);

            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified user doesn't exist.");
            }

            var country = await _unitOfWork.Countries.GetCountryWithCities(userToUpdateDto.Country);
            var city = await _unitOfWork.Cities.GetCityById(userToUpdateDto.City, userToUpdateDto.Country);
            if (city == null || country == null || !country.Cities.Contains(city))
            {
                return new KeyValuePair<bool, string>(false, "Specified country or city doesn't exist!");
            }

            user.City = city;
            user.Country = country;
            user.Email = userToUpdateDto.Email;
            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "User successfully updated.");
            }

            return new KeyValuePair<bool, string>(false, "User can't be updated.");
        }

        public async Task<KeyValuePair<bool, string>> VisitUser(VisitUserDto visitUserDto)
        {
            // Map visit user dto to visit object
            var visit = _mapper.Map<Visit>(visitUserDto);

            _unitOfWork.Users.VisitUser(visit);

            if (await _unitOfWork.Complete())
                return new KeyValuePair<bool, string>(true, "User successfully visited!");

            return new KeyValuePair<bool, string>(false, "Problem with visiting user!");
        }
    }
}