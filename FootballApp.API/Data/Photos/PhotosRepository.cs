using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Photos
{
    public class PhotosRepository : IPhotosRepository
    {
        private readonly DataContext _context;
        public PhotosRepository(DataContext context)
        {
            _context = context;

        }
        public Task<Photo> GetMainPhotoForUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            
            return photo;
        }
    }
}