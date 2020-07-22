using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Photos
{
    public class PhotosRepository : Repository<Photo>, IPhotosRepository
    {

        public PhotosRepository(DataContext context) 
            : base(context)
        {
        }
        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            var mainPhoto = await DataContext.Photos
                                         .FirstOrDefaultAsync(p => p.IsMain);

            return mainPhoto;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}