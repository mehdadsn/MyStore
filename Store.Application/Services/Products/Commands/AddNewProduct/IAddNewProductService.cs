using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.AddNewProduct
{
    public interface IAddNewProductService
    {
        public ResultDto Execute(RequestAddNewProductDto request);
    }

    public class AddNewProductService : IAddNewProductService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public AddNewProductService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public ResultDto Execute(RequestAddNewProductDto request)
        {
            try
            {
                var category = _context.Categories.Find(request.CategoryId);

                Product product = new Product()
                {
                    Name = request.Name,
                    Brand = request.Brand,
                    Description = request.Description,
                    Price = request.Price,
                    Inventory = request.Inventory,
                    Category = category,
                    Displayed = request.Displayed,
                };
                _context.Products.Add(product);

                List<ProductImages> productImages = new List<ProductImages>();
                foreach(var item in request.Images)
                {
                    var uploadResult = UploadFile(item);
                    productImages.Add(new ProductImages
                    {
                        Product = product,
                        Src = uploadResult.FileNameAddress,
                    });
                }
                _context.ProductImages.AddRange(productImages);


                List<ProductFetures> productFetures = new List<ProductFetures>();
                foreach (var item in request.Features)
                {
                    productFetures.Add(new ProductFetures()
                    {
                        DisplayName = item.DisplayName,
                        value = item.Value,
                        Product = product,
                    });
                }
                _context.ProductFetures.AddRange(productFetures);
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "محصول با موفقیت اضافه شد"
                };
            }
            catch(Exception ex) 
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            throw new NotImplementedException();
        }

        private UploadDto UploadFile(IFormFile file)
        {
            if(file != null)
            {
                string folder = $@"images\ProductImages\";
                var UploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);               
                if (!Directory.Exists(UploadsRootFolder))
                {
                    Directory.CreateDirectory(UploadsRootFolder);
                }

                if(file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string filename = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(UploadsRootFolder, filename);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
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

    public class RequestAddNewProductDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Inventory { get; set; }
        public long CategoryId { get; set; }
        public bool Displayed { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<AddNewProductFeatures> Features { get; set; }
    }

    public class AddNewProductFeatures
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }

    public class UploadDto
    {
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }
}
