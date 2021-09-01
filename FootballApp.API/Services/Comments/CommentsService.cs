using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;

namespace FootballApp.API.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<CommentToReturn>> GetAllCommentsForUser(int userId)
        {
            var comments = await _unitOfWork.Comments.GetAllCommentsForUser(userId);
            return _mapper.Map<ICollection<CommentToReturn>>(comments);
        }

        public async Task<KeyValuePair<bool, string>> PostCommentForUser(int userId, CommentForCreationDto commentForCreationDto)
        {
            var comment = _mapper.Map<Comment>(commentForCreationDto);
            _unitOfWork.Comments.Add(comment);
            
            if(await _unitOfWork.Complete())
                return new KeyValuePair<bool, string>(true, "Comment added successfully!");

            return new KeyValuePair<bool, string>(false, "Problem adding comment!");
        }
    }
}