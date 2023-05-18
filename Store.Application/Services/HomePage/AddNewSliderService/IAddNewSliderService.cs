using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.HomePage.AddNewSliderService
{
    public interface IAddNewSliderService
    {
        ResultDto Execute(IFormFile file, string link);
    }

    public class AddNewSliderService :IAddNewSliderService {

        private readonly IHostingEnvironment _environment;
        private readonly IDataBaseContext _context;
        public AddNewSliderService(IHostingEnvironment environment, IDataBaseContext context)
        {
            _context = context;
            _environment = environment;
        }
        public ResultDto Execute(IFormFile file, string link)
        {
            var uploadResult = UploadFile(file);

            Slider slider = new Slider()
            {
                Link = link,
                Src = uploadResult.FileNameAddress
            };

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "با موفقیت آپلود شد"
            };
        }

        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"images\HomePage\Slider\";
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


    public class UploadDto
    {
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }
}
