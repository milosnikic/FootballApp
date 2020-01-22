using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Photos
{
    public interface IPhotosRepository
    {
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);
    }
}