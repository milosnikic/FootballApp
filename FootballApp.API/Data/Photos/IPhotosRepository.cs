using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Photos
{
    public interface IPhotosRepository : IRepository<Photo>
    {
         Task<Photo> GetMainPhotoForUser(int userId);
         Task<ICollection<Photo>> GetAllPhotosForUser(int userId);
    }
}