﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageUploadService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploadService
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration Configuration;
        private readonly Cloudinary _cloudinary;
        private readonly ImageUploadSetting _accountSettings;

        public ImageService(IConfiguration configuration, IOptions<ImageUploadSetting> accountSettings)
        {
            Configuration = configuration;
            _accountSettings = accountSettings.Value;
            _cloudinary = new Cloudinary(new Account(_accountSettings.CloudName, _accountSettings.ApiKey, _accountSettings.ApiSecret));
        }

        /// <summary>
        /// Uploads an image to cloudinary and returns the upload result
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task<UploadResult> ImageUploadAsync(IFormFile image)
        {
            var pictureSize = Configuration.GetSection("ImageSettings:Size").Get<long>();
            bool supportedFormat = false;

            // Verifies that the image size is not larger than the preset maximum allowable size
            if (image.Length > pictureSize)
            {
                throw new ArgumentException("File size should not be more than 3Mb!");
            }

            var listOfImageFormats = Configuration.GetSection("ImageSettings:Formats").Get<List<string>>();

            // Verifies that the image format is amongst the supported formats
            foreach (var format in listOfImageFormats)
            {
                if (image.FileName.EndsWith(format))
                {
                    supportedFormat = true;
                    break;
                }
            }

            if (supportedFormat == false)
            {
                throw new ArgumentException("File format not supported!");
            }
            else
            {
                var uploadResult = new ImageUploadResult();
                using (var imageStream = image.OpenReadStream())
                {
                    //Add Guid to file Name to ensure each file have a unique name.
                    string fileName = Guid.NewGuid().ToString() + image.FileName;
                    uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, imageStream),
                        Transformation = new Transformation().Crop("thumb").Gravity("face").Width(150).Height(150)
                    });
                }
                return uploadResult;
            }
        }
    }
}
 /*  using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

var cloudinary = new Cloudinary(new Account("dnsoisble", "387598396663466", "OuzoUrZ7FIwhmhtg1tM6IXMY1vg"));

    // Upload
    var uploadParams = new ImageUploadParams()
    {
        File = new FileDescription(@"https://upload.wikimedia.org/wikipedia/commons/a/ae/Olympic_flag.jpg"),
        PublicId = "olympic_flag"
    };

    var uploadResult = cloudinary.Upload(uploadParams);

    //Transformation
    cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(100).Height(150).Crop("fill")).BuildUrl("olympic_flag")
  }
}*/