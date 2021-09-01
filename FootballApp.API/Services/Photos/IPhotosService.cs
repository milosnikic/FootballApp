using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Photos
{
    public interface IPhotosService
    {
        Task<IEnumerable<PhotoToReturnDto>> GetAllPhotosForUser(int userId);
        Task<PhotoToReturnDto> GetPhoto(int id);
        Task<PhotoToReturnDto> UploadPhoto(int userId, PhotoForCreationDto photoForCreationDto);
        Task<KeyValuePair<bool, string>> MakePhotoMain(int photoId, int userId);
    }
}