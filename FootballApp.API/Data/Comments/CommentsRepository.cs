using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Comments
{
    public class CommentsRepository : Repository<Comment>, ICommentsRepository
    {
        public CommentsRepository(DataContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Method returns all comments on specified user
        /// </summary>
        /// <param name="userId">Id of user whose comments are being retreived</param>
        /// <returns></returns>
        public async Task<ICollection<Comment>> GetAllCommentsForUser(int userId)
        {
            var comments = await DataContext.Comments
                                      .Where(c => c.CommentedId == userId)
                                      .Include(c => c.Commenter)
                                      .ThenInclude(u => u.Photos)
                                      .OrderByDescending(c => c.Created)
                                      .ToListAsync();
            return comments;
        }


        /// <summary>
        /// Method returns all comments that user created
        /// </summary>
        /// <param name="userId">Id of user that commented</param>
        /// <returns></returns>
        public async Task<ICollection<Comment>> GetAllCommentedForUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public DataContext DataContext
        {
            get
            {
                return Context as DataContext;
            }
        }
    }
}