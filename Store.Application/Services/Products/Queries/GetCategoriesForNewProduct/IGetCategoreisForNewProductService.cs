using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetCategoriesForNewProduct
{
    public interface IGetCategoreisForNewProductService
    {
        ResultDto<List<AllCategorisDto>> Execute();
    }

    public class GetCategoreisForNewProductService : IGetCategoreisForNewProductService
    {
        private readonly IDataBaseContext _context;
        public GetCategoreisForNewProductService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<AllCategorisDto>> Execute()
        {
            var categories = _context.Categories.Include(p => p.ParentCategory).Where(p => p.ParentCategoryId != null).ToList().Select(p => new AllCategorisDto()
            {
                Id = p.Id,
                Name = $"{p.ParentCategory.Name} - {p.Name}"
            }).ToList();

            return new ResultDto<List<AllCategorisDto>>
            {
                Data = categories,
                IsSuccess = true,
                Message = "",
            };
        }

    }

    public class AllCategorisDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
