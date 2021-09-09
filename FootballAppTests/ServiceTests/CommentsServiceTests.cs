using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Services.Comments;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FootballAppTests.ServiceTests
{
    public class CommentsServiceTests
    {
        private readonly CommentsService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public CommentsServiceTests()
        {
            _sut = new CommentsService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllCommentsForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var dateCommented = DateTime.Now;
            var comments = new List<Comment>()
            {
                new Comment()
                {
                    Id = 1,
                    Commented = new PowerUser(),
                    CommentedId = 1,
                    Commenter = new CommonUser(),
                    CommenterId = 2,
                    Content = "Test",
                    Created = dateCommented,
                }
            };

            var commentsToReturn = new List<CommentToReturn>()
            {
                new CommentToReturn()
                {
                    Content = "Test",
                    CommenterId = 2,
                    Commented = new UserToReturnDto(),
                    CommentedId = 1,
                    Commenter = new UserToReturnDto(),
                    DateCommented = dateCommented
                }
            };

            _unitOfWorkMock.Setup(x => x.Comments.GetAllCommentsForUser(It.IsAny<int>()))
                            .ReturnsAsync(comments);

            _mapperMock.Setup(x => x.Map<ICollection<CommentToReturn>>(It.IsAny<ICollection<Comment>>()))
                            .Returns(commentsToReturn);

            // Act
            var result = await _sut.GetAllCommentsForUser(It.IsAny<int>());

            // Assert
            var comment = result.FirstOrDefault();
            Assert.Equal(comments.FirstOrDefault().Created, comment.DateCommented);
            Assert.Equal(comments.FirstOrDefault().Content, comment.Content);
            Assert.Equal(comments.FirstOrDefault().CommenterId, comment.CommenterId);
            Assert.Equal(comments.FirstOrDefault().CommentedId, comment.CommentedId);
        }

        [Fact]
        public async Task PostCommentForUser_ShouldNotAdd_WhenNotSaved()
        {
            // Arrange
            var responseMessage = "Problem adding comment!";
            var dateCommented = DateTime.Now;
            var comment = new Comment()
            {
                Id = 1,
                Commented = new PowerUser(),
                CommentedId = 1,
                Commenter = new CommonUser(),
                CommenterId = 2,
                Content = "Test",
                Created = dateCommented,
            };

            var commentForCreation = new CommentForCreationDto()
            {
                Content = "Test",
                CommenterId = 2,
                CommentedId = 1,
                DateCommented = dateCommented
            };

            _mapperMock.Setup(x => x.Map<Comment>(commentForCreation))
                            .Returns(comment);
            
            _unitOfWorkMock.Setup(x => x.Comments.Add(comment))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(false);

            // Act
            var result = await _sut.PostCommentForUser(It.IsAny<int>(), commentForCreation);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task PostCommentForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = "Comment added successfully!";
            var dateCommented = DateTime.Now;
            var comment = new Comment()
            {
                Id = 1,
                Commented = new PowerUser(),
                CommentedId = 1,
                Commenter = new CommonUser(),
                CommenterId = 2,
                Content = "Test",
                Created = dateCommented,
            };

            var commentForCreation = new CommentForCreationDto()
            {
                Content = "Test",
                CommenterId = 2,
                CommentedId = 1,
                DateCommented = dateCommented
            };

            _mapperMock.Setup(x => x.Map<Comment>(commentForCreation))
                            .Returns(comment);

            _unitOfWorkMock.Setup(x => x.Comments.Add(comment))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(true);

            // Act
            var result = await _sut.PostCommentForUser(It.IsAny<int>(), commentForCreation);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }
    }
}
