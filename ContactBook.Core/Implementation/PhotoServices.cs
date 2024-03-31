using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Core.Services;
using ContactBook.Infrastructure.Helper;

namespace ContactBook.Core.Implementation
{
    public class PhotoServices
    {
        public class PhotoService : IPhotoServices
        {
            private readonly Cloudinary _cloudinary;
            public PhotoService(IOptions<CloudinarySettings> config)
            {
                var acc = new Account
                (
                    config.Value.CloudName,
                    config.Value.ApiKey,
                    config.Value.ApiSecret
                );

                _cloudinary = new Cloudinary(acc);

            }

            public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
            {

                var uploadResult = new ImageUploadResult(); //method for uploading images and cloudinary returns this

                if (file.Length > 0)   //checks if there is atleast 1 file
                {

                    using var stream = file.OpenReadStream(); //read in streams

                    var uploadparams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),     //creats the name of the file that is uploaded
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")  //transforms the image
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadparams);    //uploads the file to cloudinary

                }

                return uploadResult;
            }

            public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
            {
                throw new NotImplementedException();
            }
        }
    }
}
