using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EGPS.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EGPS.Application.Services
{
    public class PhotoAcessor : IPhotoAcessor
    {
        private readonly Cloudinary _cloudinary;
        public PhotoAcessor(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CLOUDINARY_CLOUDNAME"],
                configuration["CLOUDINARY_APIKEY"],
                configuration["CLOUDINARY_SECRET"]
                );
            _cloudinary = new Cloudinary(account);
        }
        public ImageUploadResult AddPhoto(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();
            if (photo.Length > 0)
            {
                using (var stream = photo.OpenReadStream())
                {
                    var uploadparams = new ImageUploadParams
                    {
                        File = new FileDescription(photo.FileName, stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadparams);
                }
            }

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return uploadResult;
        }

        public RawUploadResult AddFile(IFormFile file)
        {
            var uploadFileResult = new RawUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream)
                    };
                    uploadFileResult = _cloudinary.Upload(uploadParams);
                }
            }
            if (uploadFileResult.Error != null)
            {
                throw new Exception(uploadFileResult.Error.Message);
            }
            return uploadFileResult;
        }
    }
}
