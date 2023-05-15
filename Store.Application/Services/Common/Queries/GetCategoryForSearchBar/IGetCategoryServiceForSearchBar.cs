using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Common.Queries.GetCategoryForSearchBar
{
    public interface IGetCategoryServiceForSearchBar
    {
        ResultDto<List<CategoryDto>> Execute();
    }
    public class GetCategoryServiceForSearchBar : IGetCategoryServiceForSearchBar
    {
        private readonly IDataBaseContext _context;
        public GetCategoryServiceForSearchBar(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<CategoryDto>> Execute()
        {
            var categories = _context.Categories.Where(p => p.ParentCategoryId == null)
                .ToList()
                .Select(p => new CategoryDto
                {
                     CategoryId = p.Id,
                     CategoryName = p.Name,
                }).ToList();

            return new ResultDto<List<CategoryDto>>()
            {
                Data = categories,
                IsSuccess = true
            };
        }
    }

    public class CategoryDto
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
