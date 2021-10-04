using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Comments
{
    public interface ICommentsService
    {
        Task<ICollection<CommentToReturn>> GetAllCommentsForUser(int userId);
        Task<KeyValuePair<bool, string>> PostCommentForUser(int userId, CommentForCreationDto commentForCreationDto);
    }
}