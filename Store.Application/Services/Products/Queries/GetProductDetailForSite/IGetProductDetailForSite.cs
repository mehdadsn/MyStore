using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductDetailForSite
{
    public  interface IGetProductDetailForSite
    {
        ResultDto<ProductDetailForSiteDto> Execute(long id);
    }
    public class GetProductDetailForSite : IGetProductDetailForSite
    {
        private readonly IDataBaseContext _context;
        public GetProductDetailForSite(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ProductDetailForSiteDto> Execute(long id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .ThenInclude(p => p.ParentCategory)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductFeatures)
                .Where(p => p.Id == id).FirstOrDefault();
            if(product == null)
            {
                throw new Exception("Product Not Found...");
            }

            product.ViewCount++;
            _context.SaveChanges();

            return new ResultDto<ProductDetailForSiteDto>
            {
                Data = new ProductDetailForSiteDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand,
                    Category = $"{product.Category.ParentCategory.Name} - {product.Category.Name}",
                    Description = product.Description,
                    Price = product.Price,
                    ImageSrcs = product.ProductImages.Select(p => p.Src).ToList(),
                    Features = product.ProductFeatures.Select(p => new ProductDetailForSite_FeaturesDto
                    {
                        DisplayName = p.DisplayName,
                        Value = p.value,
                    }).ToList(),

                },
                IsSuccess = true,
            };
        }
    }

    public class ProductDetailForSiteDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public float Price { get; set; }
        public List<string> ImageSrcs { get; set; }
        public List<ProductDetailForSite_FeaturesDto> Features { get; set; }

    }

    public class ProductDetailForSite_FeaturesDto
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }
}
