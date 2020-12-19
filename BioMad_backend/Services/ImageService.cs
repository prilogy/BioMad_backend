﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BioMad_backend.Data;
using BioMad_backend.Entities;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BioMad_backend.Services
{
    public class ImageService
    {
        private readonly ApplicationContext _db;
        private readonly IWebHostEnvironment _appEnvironment;

        public ImageService(ApplicationContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }

        public async Task<Image> AddAsync(IFormFile file)
        {
            if (file == null) return null;

            try
            {
                using var algorithm = new Rfc2898DeriveBytes(
                    DateTime.UtcNow.ToString(),
                    8,
                    10,
                    HashAlgorithmName.SHA512);

                var name = Regex.Replace(Convert.ToBase64String(algorithm.GetBytes(5)) + file.FileName, @"(\s|\\.|\/|\\)+", "");

                var path = "/static/" + name;
                var systemPath = _appEnvironment.WebRootPath + path;

                await using (var fileStream = new FileStream(systemPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Position = 0;
                    var optimizer = new ImageOptimizer
                    {
                        OptimalCompression = true,
                        IgnoreUnsupportedFormats = true
                    };
          
                    optimizer.Compress(fileStream);
                }

                var magickImage = new MagickImage(systemPath) {Quality = 40};
                magickImage.Strip();
                magickImage.Write(systemPath);

                var image = new Image {Path = path};
                await _db.Images.AddAsync(image);
                await _db.SaveChangesAsync();

                return image;
            }
            catch
            {
                return null;
            }
        }
    }
}