

using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Services.Photos;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FootballApp.API.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FootballAppTests.ServiceTests
{
    public class PhotosServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly PhotosService _sut;

        public PhotosServiceTests()
        {
            _sut = new PhotosService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllPhotosForUser_ShouldReturnAllPhotos()
        {
            // Arrange
            var photoId = 1;
            var userId = 1;
            var dateAdded = DateTime.Now;
            var description = "Test";
            var photo = new Photo()
            {
                Id = photoId,
                Description = description,
                DateAdded = dateAdded
            };

            var photos = new List<PhotoToReturnDto>()
            {
                new PhotoToReturnDto()
                {
                    Id = photoId,
                    DateAdded = dateAdded,
                    Description = description
                }
            };

            _unitOfWorkMock.Setup(x => x.Photos.GetAllPhotosForUser(userId))
                        .ReturnsAsync(It.IsAny<ICollection<Photo>>());

            _mapperMock.Setup(x => x.Map<ICollection<PhotoToReturnDto>>(It.IsAny<ICollection<Photo>>()))
                        .Returns(photos);
            // Act
            var result = await _sut.GetAllPhotosForUser(userId);

            // Assert
            var resultPhoto = result.FirstOrDefault();
            Assert.Equal(photoId, resultPhoto.Id);
            Assert.Equal(dateAdded, resultPhoto.DateAdded);
            Assert.Equal(description, resultPhoto.Description);
        }


        [Fact]
        public async Task GetPhoto_ShouldReturnPhoto()
        {
            // Arrange
            var photoId = 1;
            var dateAdded = DateTime.Now;
            var description = "Test";
            var photo = new Photo()
            {
                Id = photoId,
                Description = description,
                DateAdded = dateAdded
            };

            var photoToReturn = new PhotoToReturnDto()
            {
                Id = photoId,
                DateAdded = dateAdded,
                Description = description
            };

            _unitOfWorkMock.Setup(x => x.Photos.GetById(It.IsAny<int>()))
                        .ReturnsAsync(photo);

            _mapperMock.Setup(x => x.Map<PhotoToReturnDto>(photo))
                        .Returns(photoToReturn);
            // Act
            var result = await _sut.GetPhoto(photoId);

            // Assert
            Assert.Equal(photoId, result.Id);
            Assert.Equal(dateAdded, result.DateAdded);
            Assert.Equal(description, result.Description);
        }

        [Fact]
        public async Task MakePhotoMain_ShouldNotMake_WhenPhotoDoesNotExist()
        {
            // Arrange
            var responseMessage = "Specified photo doesn't exist.";
            var photoId = 1;
            var userId = 1;

            _unitOfWorkMock.Setup(x => x.Photos.GetById(photoId))
                        .ReturnsAsync(() => null);

            // Act
            var result = await _sut.MakePhotoMain(photoId, userId);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task MakePhotoMain_ShouldNotMake_WhenNotSaved()
        {
            // Arrange
            var responseMessage = "Problem setting photo to main.";
            var photoId = 1;
            var userId = 1;
            var dateAdded = DateTime.Now;
            var description = "Test";
            var photo = new Photo()
            {
                Id = photoId,
                Description = description,
                DateAdded = dateAdded
            };

            _unitOfWorkMock.Setup(x => x.Photos.GetById(photoId))
                        .ReturnsAsync(photo);

            _unitOfWorkMock.Setup(x => x.Photos.GetMainPhotoForUser(userId))
                        .ReturnsAsync(It.IsAny<Photo>());

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(false);

            // Act
            var result = await _sut.MakePhotoMain(photoId, userId);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task MakePhotoMain_ShouldMake_WhenMainPhotoIsNull()
        {
            // Arrange
            var responseMessage = "Photo successfully set to main.";
            var photoId = 1;
            var userId = 1;
            var dateAdded = DateTime.Now;
            var description = "Test";
            var photo = new Photo()
            {
                Id = photoId,
                Description = description,
                DateAdded = dateAdded
            };

            var photoToReturn = new PhotoToReturnDto()
            {
                Id = photoId,
                DateAdded = dateAdded,
                Description = description
            };

            _unitOfWorkMock.Setup(x => x.Photos.GetById(photoId))
                        .ReturnsAsync(photo);

            _unitOfWorkMock.Setup(x => x.Photos.GetMainPhotoForUser(userId))
                        .ReturnsAsync(() => null);

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(true);

            // Act
            var result = await _sut.MakePhotoMain(photoId, userId);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task MakePhotoMain_ShouldMake_WhenMainPhotoIsNotNull()
        {
            // Arrange
            var responseMessage = "Photo successfully set to main.";
            var photoId = 1;
            var userId = 1;
            var dateAdded = DateTime.Now;
            var description = "Test";
            var mainPhotoDescription = "Main photo";
            var photo = new Photo()
            {
                Id = photoId,
                Description = description,
                DateAdded = dateAdded
            };

            var mainPhoto = new Photo()
            {
                Id = photoId,
                Description = mainPhotoDescription,
                DateAdded = dateAdded
            };

            var photoToReturn = new PhotoToReturnDto()
            {
                Id = photoId,
                DateAdded = dateAdded,
                Description = description
            };

            _unitOfWorkMock.Setup(x => x.Photos.GetById(photoId))
                        .ReturnsAsync(photo);

            _unitOfWorkMock.Setup(x => x.Photos.GetMainPhotoForUser(userId))
                        .ReturnsAsync(mainPhoto);

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(true);

            // Act
            var result = await _sut.MakePhotoMain(photoId, userId);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        //[Fact]
        //public async Task UploadPhoto_ShouldUpload_WhenThereIsNoMainPhoto()
        //{
        //    // Arrange
        //    var userId = 1;
        //    var photoId = 1;
        //    var fileMock = new Mock<IFormFile>();
        //    var physicalFile = new FileInfo("../Files/Test_image.jpg");
        //    var ms = new MemoryStream();
        //    var writer = new StreamWriter(ms);
        //    var dateAdded = DateTime.Now;
        //    var description = "test";
        //    var isMain = true;
        //    writer.Write(physicalFile.OpenRead());
        //    writer.Flush();
        //    ms.Position = 0;
        //    var fileName = physicalFile.Name;
        //    //Setup mock file using info from physical file
        //    fileMock.Setup(_ => _.FileName).Returns(fileName);
        //    fileMock.Setup(_ => _.Length).Returns(ms.Length);
        //    fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        //    fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));

        //    var file = fileMock.Object;
        //    var photoForCreationDto = new PhotoForCreationDto()
        //    {
        //        File = file,
        //        DateAdded = dateAdded,
        //        Description = description,
        //        IsMain = isMain
        //    };

        //    var photo = new Photo()
        //    {
        //        Id = photoId,
        //        IsMain = isMain,
        //        DateAdded = dateAdded
        //    };

        //    var photoToReturn = new PhotoToReturnDto()
        //    { 
        //        Id = photoId,
        //        IsMain = isMain,
        //        DateAdded = dateAdded
        //    };

        //    _mapperMock.Setup(x => x.Map<Photo>(photoForCreationDto))
        //                .Returns(photo);

        //    _unitOfWorkMock.Setup(x => x.Photos.GetMainPhotoForUser(userId))
        //                .ReturnsAsync(() => null);

        //    _unitOfWorkMock.Setup(x => x.Photos.Add(photo))
        //                .Verifiable();

        //    _unitOfWorkMock.Setup(x => x.Complete())
        //                .ReturnsAsync(true);

        //    _mapperMock.Setup(x => x.Map<PhotoToReturnDto>(photo))
        //                .Returns(photoToReturn);

        //    // Act
        //    var result = await _sut.UploadPhoto(userId, photoForCreationDto);

        //    // Assert
        //    Assert.Equal(result.Id, photoId);
        //    Assert.Equal(result.DateAdded, dateAdded);
        //    Assert.Equal(result.IsMain, isMain);
        //}
    }
}
