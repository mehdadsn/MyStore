using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.DeleteProduct
{
    public interface IDeleteProductService
    {
        ResultDto Execute(long Id);
    }

    public class DeleteProductService : IDeleteProductService
    {
        private readonly IDataBaseContext _context;
        public DeleteProductService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long Id)
        {
            var product = _context.Products.Find(Id);
            if (product == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "محصول مورد نظر پیدا نشد",
                };
            }
            product.IsRemoved = true;
            _context.SaveChanges();
            throw new NotImplementedException();
        }
    }
}
