using Store.Application.Interface.Context;
using Store.Common.Dto;

namespace Store.Application.Services.Products.Commands.DeleteCategory
{
    public class DeleteCategoryService : IDeleteCategoryService
    {
        private readonly IDataBaseContext _context;
        public DeleteCategoryService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long CategoryId)
        {
            var category = _context.Categories.Find(CategoryId);
            if(category == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دسته بندی مورد نظر پیدا نشد!"
                };
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return new ResultDto { IsSuccess = true, Message = "دسته بندی با موفقیت حذف شد" };
        }
    }
}
