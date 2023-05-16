using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductsForSite
{
    public interface IGetProductsForSite
    {
        ResultDto<ResultProductsForSiteDto> Execute(Ordering ordering, string searchKey, int page, int pageSize, long? CategoryId = null);
    }

    public class GetProductsForSite : IGetProductsForSite
    {
        private readonly IDataBaseContext _context;
        public GetProductsForSite(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultProductsForSiteDto> Execute(Ordering ordering, string searchKey, int page,int pageSize, long? CategoryId = null)           
        {
            int totalRow = 0;
            var productsQuery = _context.Products
                .Include(p => p.ProductImages).AsQueryable();

            if(CategoryId != null)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == CategoryId || p.Category.ParentCategoryId == CategoryId).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchKey) || p.Brand.Contains(searchKey)).AsQueryable();
            }

            switch (ordering)
            {
                case Ordering.NoOrder:
                    productsQuery = productsQuery.OrderByDescending(p => p.Id).AsQueryable();
                    break;
                case Ordering.MostVisited:
                    productsQuery = productsQuery.OrderByDescending(p => p.ViewCount).AsQueryable();
                    break;
                case Ordering.BestSelling:
                    break;
                case Ordering.MostPopular:
                    break;
                case Ordering.Newest:
                    productsQuery = productsQuery.OrderByDescending(p => p.Id).AsQueryable();
                    break;
                case Ordering.Cheapest:
                    productsQuery = productsQuery.OrderBy(p => p.Price).AsQueryable();
                    break;
                case Ordering.MostExpensive:
                    productsQuery = productsQuery.OrderByDescending(p => p.Price).AsQueryable();
                    break;
            }



            var products = productsQuery.ToPaged(page, pageSize, out totalRow);

            Random rd = new Random();

            return new ResultDto<ResultProductsForSiteDto>
            {
                Data = new ResultProductsForSiteDto()
                {
                    TotalRows = totalRow,
                    Products = products.Select(p => new ProductForSiteDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Star = rd.Next(1,5),
                        Price = p.Price,
                        ImageSrc = p.ProductImages.FirstOrDefault().Src,
                    }).ToList(),
                },
                IsSuccess = true,

            };
        }
    }

    public class ResultProductsForSiteDto
    {
        public List<ProductForSiteDto> Products { get; set; }
        public int TotalRows { get; set; }

    }

    public class ProductForSiteDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string ImageSrc { get; set; }
        public int Star { get; set; }
    }

    public enum Ordering
    {
        NoOrder = 0,
        MostVisited = 1,
        BestSelling = 2,
        MostPopular = 3,
        Newest = 4,
        Cheapest = 5,
        MostExpensive = 6
    }
}
