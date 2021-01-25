using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using CloudinaryDotNet.Actions;

namespace EGPS.Application.Interfaces
{
    public interface IPhotoAcessor
    {
        ImageUploadResult AddPhoto(IFormFile photo);
        RawUploadResult AddFile(IFormFile file);
    }
}
