using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Comments
{
    public interface ICommentsRepository : IRepository<Comment>
    {
        Task<ICollection<Comment>> GetAllCommentsForUser(int userId);
        Task<ICollection<Comment>> GetAllCommentedForUser(int userId);
    }
}