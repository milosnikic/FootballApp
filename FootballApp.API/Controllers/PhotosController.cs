using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Services.Photos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotosService _photosService;
        public PhotosController(IPhotosService photosService)
        {
            _photosService = photosService;
        }

        [HttpGet]
        [AllowAnonymous]
        //We have to allow anonymous because we want other people to see photos    
        public async Task<IActionResult> GetAllPhotosForUser(int userId)
        {
            return Ok(await _photosService.GetAllPhotosForUser(userId));
        }


        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            return Ok(await _photosService.GetPhoto(id));
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

            var photo = await _photosService.UploadPhoto(userId, photoForCreationDto);
            if (photo == null)
                return Ok(new KeyValuePair<bool, string>(false, "File is not allowed"));

            return Ok(photo);
        }

        [HttpPost]
        [Route("main")]
        public async Task<IActionResult> MakePhotoMain(int photoId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            return Ok(await _photosService.MakePhotoMain(photoId, userId));
        }
    }
}