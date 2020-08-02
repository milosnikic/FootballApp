using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FootballApp.API.Helpers
{
    /// <summary>
    /// Class used to do some image validations
    /// </summary>
    internal static class ImageValidator
    {
        /// <summary>
        /// Method checks if image size is greater than 0 and less than 5MB
        /// </summary>
        /// <param name="image">Image file to check</param>
        /// <returns></returns>
        internal static bool ValidateImageSize(IFormFile image)
        {
            return image.Length > 0 && image.Length < 5242880;
        }

        /// <summary>
        /// Method checks if image extensions is valid
        /// </summary>
        /// <param name="image">Image file to check</param>
        /// <returns>Returns true if image extension is .jpg, .jpeg, .png</returns>
        internal static bool ValidateImageExtension(IFormFile image)
        {
            string[] permittedExtensions = { ".jpeg", ".jpg", ".png" };

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();

            return !string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext);
        }

        /// <summary>
        /// Method checks if image signature (first bytes) is valid
        /// </summary>
        /// <param name="image">Image file to check</param>
        /// <returns>Returns true if image signature is valid</returns>
        internal static bool ValidateImageSignature(IFormFile image)
        {
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (ext == ".jpg")
            {
                return true;
            }
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
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }

        }
    }
}