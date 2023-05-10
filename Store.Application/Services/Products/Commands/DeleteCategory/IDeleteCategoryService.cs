using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.DeleteCategory
{
    public interface IDeleteCategoryService
    {
        ResultDto Execute(long CategoryId);
    }
}
