using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.EditProduct
{
    public interface IEditProductService
    {
        ResultDto Execute(RequestEditProductDto request);
    }

    public class EditProductService : IEditProductService
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public EditProductService(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public ResultDto Execute(RequestEditProductDto request)
        {
            try
            {
                var category = _context.Categories.Find(request.CategoryId);

                var product = _context.Products.Where(p => p.Id == request.ProductId)
                    .Include(p => p.ProductFeatures)
                    .Include(p => p.ProductImages)
                    .FirstOrDefault();

                //var product = _context.Products.Include(p => p.ProductFeatures).Include(p => p.ProductImages)
                //    .Where(p => p.Id == request.ProductId)
                //    .Select(p => new RequestEditProductDto
                //{
                //Name = p.Name,
                //Description = p.Description,
                //CategoryId = p.CategoryId,
                //Inventory = p.Inventory,
                //Price = p.Price,
                //Displayed = p.Displayed,
                //}).ToList();
                
                if (product == null)
                {
                    return new ResultDto()
                    {
                        IsSuccess = false,
                        Message = "محصول مورد نظر یافت نشد!"
                    };
                }
                product.Name = request.Name;
                product.Description = request.Description;
                product.CategoryId = request.CategoryId;
                product.Category = category;
                product.Inventory = request.Inventory;
                product.Price = request.Price;
                product.Displayed = request.Displayed;

                List<ProductImages> productImages = new List<ProductImages>();
                foreach (var item in request.Images)
                {
                    bool exixst = false;
                     foreach(var image in product.ProductImages)
                     {
                        if (image.Src.Contains(item.FileName))
                        {
                            exixst = true;
                        }                      
       
                     }
                     if (!exixst) {
                        var uploadResult = UploadFile(item);
                        productImages.Add(new ProductImages
                        {
                            Product = product,
                            Src = uploadResult.FileNameAddress,
                            InsertTime = DateTime.Now,
                        });
                    }
                }
                _context.ProductImages.AddRange(productImages);

                foreach(var feature in product.ProductFeatures)
                {
                    _context.ProductFeatures.Remove(feature);
                }

                List<ProductFeatures> productFeatures = new List<ProductFeatures>();
                foreach (var feature in request.Features)
                {
                    productFeatures.Add(new ProductFeatures
                    {
                        DisplayName = feature.DisplayName,
                        value = feature.Value,
                        Product = product,
                        InsertTime= DateTime.Now,
                    });
                }
                _context.ProductFeatures.AddRange(productFeatures);
                _context.SaveChanges();

                return new ResultDto()
                {
                    Message = "با موفقیت ویرایش شد",
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
                       
        }
        public UploadDto UploadFile(IFormFile file)
        {
            if(file != null)
            {
                string folder = $@"images\ProductImages\";
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }

                if(file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filepath = Path.Combine(uploadsRootFolder, fileName);
                using(var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return new UploadDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                };
            }
            return null;
        }

    }

    public class RequestEditProductDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Inventory { get; set; }
        public long CategoryId { get; set; }
        public bool Displayed { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<EditProductFeatures> Features { get; set; }
    }

    public class EditProductFeatures
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
