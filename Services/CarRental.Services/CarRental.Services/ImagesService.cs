using CarRental.Services.Contracts;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CarRental.Services
{
    public class ImagesService : IImagesService
    {
        public  async Task<string> UploadImage(Cloudinary cloudinary, IFormFile imageSource, string imageName)
        {
            byte[] destinationImage;
            using (var memoryStream = new MemoryStream())
            {
                await imageSource.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            using (var ms = new MemoryStream(destinationImage))
            {
                // Cloudinary doesn't work with &
                imageName = imageName.Replace("&", "And");

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageName, ms),
                    PublicId = imageName,
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.SecureUri.AbsoluteUri;
            }
        }
    }
}
