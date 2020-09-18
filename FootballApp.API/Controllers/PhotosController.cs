using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Helpers;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PhotosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPhotosForUser(int userId)
        {
            //We have to allow anonymous because we want other people to see photos    
            var photos = await _unitOfWork.Photos.GetAllPhotosForUser(userId);

            return Ok(_mapper.Map<ICollection<PhotoToReturnDto>>(photos));
        }


        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _unitOfWork.Photos.GetById(id);

            var photo = _mapper.Map<PhotoToReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        /// <summary>
        /// Method is used to upload user photo, if its set to be main photo, it will replace
        /// current main photo
        /// </summary>
        /// <param name="userId">Id of user whose photo is being upload</param>
        /// <param name="photoForCreationDto">All needed params for image upload</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(int userId,
            [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var image = photoForCreationDto.File;


            if (ImageValidator.ValidateImageSize(image)
                && ImageValidator.ValidateImageExtension(image)
                && ImageValidator.ValidateImageSignature(image))
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
                        return Ok(_mapper.Map<PhotoToReturnDto>(photo));
                    }
                }
            }
            return Ok(new KeyValuePair<bool, string>(false, "File is not allowed"));
        }

        [HttpPost]
        [Route("main")]
        public async Task<IActionResult> MakePhotoMain(int photoId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            // Find specified photo
            var photo = await _unitOfWork.Photos.GetById(photoId);
            if (photo == null)
                return BadRequest("Specified photo doesn't exist.");
            
            // Check if user has profile photo
            // If does change it to not be profile
            var mainPhoto = await _unitOfWork.Photos.GetMainPhotoForUser(userId);
            if (mainPhoto != null)
                mainPhoto.IsMain = false;

            photo.IsMain = true;
            if (await _unitOfWork.Complete())
                return Ok(new KeyValuePair<bool, string>(true, "Photo successfully set to main."));

            return BadRequest("Problem setting photo to main.");
        }
    }
}