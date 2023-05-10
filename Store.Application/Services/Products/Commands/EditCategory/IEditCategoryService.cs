using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.EditCategory
{
    public interface IEditCategoryService
    {
        public ResultDto Execute(RequestEditCategoryDto request);
    }

    public class EditCategoryService : IEditCategoryService
    {
        private readonly IDataBaseContext _context;
        public EditCategoryService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestEditCategoryDto request)
        {
            var category = _context.Categories.Find(request.CategoryId);
            if (category == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دسته بندی پیدا نشد!"
                };
            }

            category.Name = request.Name;
            _context.SaveChanges();

            return new ResultDto { IsSuccess = true, Message = "دسته بندی با موفقیت ویرایش شد" };
        }
    }

    public class RequestEditCategoryDto
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
    }
}
