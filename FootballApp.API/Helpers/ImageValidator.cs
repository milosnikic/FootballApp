using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FootballApp.API.Helpers
{
    public static class ImageValidator
    {
        /// <summary>
        /// This method checks if there is image and if image size is less than 10MB.
        /// </summary>
        /// <param name="image">Image file to be checked.</param>
        /// <returns>True if image size is valid.</returns>
        public static bool ImageSizeValidation(IFormFile image)
        {
            return image.Length > 0 && image.Length < 10485760;
        }

        /// <summary>
        /// This method checks if file extension is allowed.
        /// </summary>
        /// <param name="image">Image file to be checked.</param>
        /// <returns>True if image extension is in allowed values.</returns>
        public static bool ImageExtensionValidation(IFormFile image)
        {
            string[] permittedExtensions = { ".png", ".jpeg", ".jpg" };

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

            return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
        }

        /// <summary>
        /// This method checks if image signature is valid.
        /// </summary>
        /// <param name="image">Image file to be checked.</param>
        /// <returns>True if image signature is allowed.</returns>
        public static bool ImageSignatureValidation(IFormFile image)
        {
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

            Dictionary<string, List<byte[]>> _fileSignature =
                new Dictionary<string, List<byte[]>>
                {
                    { ".jpeg", new List<byte[]>
                        {
                            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                            new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                            new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                        }
                    },
                    { ".png", new List<byte[]>
                        {
                            new byte[] { 0x89, 0x50, 0x4E, 0x47 }
                        }
                    }
                };

            using (var reader = new BinaryReader(image.OpenReadStream()))
            {
                var signatures = _fileSignature[ext != ".jpg" ? ext : ".jpeg"];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            };

        }

        /// <summary>
        /// Method is used to get image byte are from original image file
        /// </summary>
        /// <param name="image">Image file</param>
        /// <returns>Image byte array if validations are passed otherwise null</returns>
        public static async Task<byte[]> GetImageArrayFromImageFileAsync(IFormFile image)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    if (image != null &&
                        ImageValidator.ImageSizeValidation(image) &&
                        ImageValidator.ImageExtensionValidation(image) &&
                        ImageValidator.ImageSignatureValidation(image))
                    {
                        await image.CopyToAsync(memoryStream);

                        return memoryStream.ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
