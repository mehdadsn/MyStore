using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.HomePage.AddHomepageImages
{
    public interface IAddHomepageImagesService
    {
        ResultDto Execute(RequestAddHomepageImagesDto request);
    }

    public class AddHomepageImagesService : IAddHomepageImagesService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IDataBaseContext _context;
        public AddHomepageImagesService(IHostingEnvironment environment, IDataBaseContext context)
        {
            _context = context;
            _environment = environment;
        }
        public ResultDto Execute(RequestAddHomepageImagesDto request)
        {
            var uploadResult = UploadFile(request.File);
            HomePageImages homePageImages = new HomePageImages()
            {
                Link = request.Link,
                Src = uploadResult.FileNameAddress,
                ImageLocation = request.ImageLocation,
            };
            _context.HomePageImages.Add(homePageImages);
            _context.SaveChanges();

            return new ResultDto() 
            { 
                IsSuccess = true, Message = "با موفقیت افزوده شد"   
            };
        }

        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"images\HomePage\Images\";
                var UploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(UploadsRootFolder))
                {
                    Directory.CreateDirectory(UploadsRootFolder);
                }

                if (file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string filename = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(UploadsRootFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return new UploadDto()
                {
                    Status = true,
                    FileNameAddress = folder + filename
                };
            }
            return null;
        }
    }
    public class RequestAddHomepageImagesDto
    {
        public IFormFile File { get; set; }
        public string Link { get; set; }
        public ImageLocation ImageLocation { get; set; }
    }

    public class UploadDto
    {
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }
}
