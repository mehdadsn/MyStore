using Store.Application.Services.Products.Commands.AddNewCategory;
using Store.Application.Services.Products.Commands.DeleteCategory;
using Store.Application.Services.Products.Commands.EditCategory;
using Store.Application.Services.Products.Queries.GetCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Interface.FacadPatterns
{
    public interface IProductFacad
    {
        AddNewCategoryService AddNewCategoryService { get; }
        IGetCategoriesService GetCategoriesService { get; }
        IDeleteCategoryService DeleteCategoryService { get; }
        IEditCategoryService EditCategoryService { get; }
    }
}
