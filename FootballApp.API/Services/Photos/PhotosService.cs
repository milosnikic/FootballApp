using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Helpers;
using FootballApp.API.Models;

namespace FootballApp.API.Services.Photos
{
    public class PhotosService : IPhotosService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PhotosService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PhotoToReturnDto>> GetAllPhotosForUser(int userId)
        {
            var photos = await _unitOfWork.Photos.GetAllPhotosForUser(userId);

            return _mapper.Map<ICollection<PhotoToReturnDto>>(photos);
        }

        public async Task<PhotoToReturnDto> GetPhoto(int id)
        {
            var photoFromRepo = await _unitOfWork.Photos.GetById(id);

            var photo = _mapper.Map<PhotoToReturnDto>(photoFromRepo);

            return photo;
        }

        public async Task<KeyValuePair<bool, string>> MakePhotoMain(int photoId, int userId)
        {
            // Find specified photo
            var photo = await _unitOfWork.Photos.GetById(photoId);
            if (photo == null)
                return new KeyValuePair<bool, string>(false, "Specified photo doesn't exist.");
            
            // Check if user has profile photo
            // If does change it to not be profile
            var mainPhoto = await _unitOfWork.Photos.GetMainPhotoForUser(userId);
            if (mainPhoto != null)
                mainPhoto.IsMain = false;

            photo.IsMain = true;
            if (await _unitOfWork.Complete())
                return new KeyValuePair<bool, string>(true, "Photo successfully set to main.");

            return new KeyValuePair<bool, string>(false, "Problem setting photo to main.");
        }

        public async Task<PhotoToReturnDto> UploadPhoto(int userId, PhotoForCreationDto photoForCreationDto)
        {
            var image = photoForCreationDto.File;


            if (ImageValidator.ImageExtensionValidation(image)
                && ImageValidator.ImageSizeValidation(image)
                && ImageValidator.ImageSignatureValidation(image))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photoForCreationDto.File.CopyToAsync(memoryStream);

                    var photo = _mapper.Map<Photo>(photoForCreationDto);

                    photo.Image = memoryStream.ToArray();
                    photo.UserId = userId;

                    // Check if photo that is going to be uploaded is main profile photo
                    // If it is, we have to find current main photo, and change it to not be main
                    var currentMainPhoto = await _unitOfWork.Photos.GetMainPhotoForUser(userId);
                    if(photo.IsMain && currentMainPhoto != null)
                    {
                        currentMainPhoto.IsMain = false;
                    }
                    _unitOfWork.Photos.Add(photo);

                    if (await _unitOfWork.Complete())
                    {
                        return _mapper.Map<PhotoToReturnDto>(photo);
                    }
                }
            }
            return null;
        }
    }
}