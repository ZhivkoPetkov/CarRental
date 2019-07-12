using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CarRental.Services.Contracts
{
    public interface IImagesService
    {
        Task<string> UploadImage(Cloudinary cloudinary, IFormFile imageSource, string imageName);
    }
}
