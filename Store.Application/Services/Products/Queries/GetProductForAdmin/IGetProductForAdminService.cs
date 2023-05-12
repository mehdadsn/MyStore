using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductForAdmin
{
    public interface IGetProductForAdminService
    {
        public ResultDto<ProductForAdminDto> Execute(int page = 1, int pageSize = 20);
    }

    public class GetProductForAdminService : IGetProductForAdminService
    {
        private readonly IDataBaseContext _context;
        public GetProductForAdminService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ProductForAdminDto> Execute(int page = 1, int pageSize = 20)
        {
            int rowCount = 0;
            var products = _context.Products.Include(p => p.Category).ToPaged(page, pageSize, out rowCount).Select(p => new ProductsFormAdminListDto
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Category = p.Category.Name,
                Description = p.Description,
                Displayed = p.Displayed,
                Inventory = p.Inventory,
                Price = p.Price
            }).ToList();

            return new ResultDto<ProductForAdminDto>()
            {
                Data = new ProductForAdminDto()
                {
                    Products = products,
                    CurrentPage = page
                },
                IsSuccess = true,
                Message = "",
            };
        }
    }

    public class ProductForAdminDto
    {
        public int RowCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<ProductsFormAdminListDto> Products { get; set; }
    }

    public class ProductsFormAdminListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Inventory { get; set; }
        public bool Displayed { get; set; }
    }
}
