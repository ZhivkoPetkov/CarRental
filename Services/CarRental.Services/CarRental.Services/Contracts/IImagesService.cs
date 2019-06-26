using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services.Contracts
{
    public interface IImagesService
    {
        Task<string> UploadImage(Cloudinary cloudinary, IFormFile imageSource, string imageName);
    }
}
