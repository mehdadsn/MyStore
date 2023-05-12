using Store.Application.Services.Products.Commands.AddNewCategory;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Commands.DeleteCategory;
using Store.Application.Services.Products.Commands.DeleteProduct;
using Store.Application.Services.Products.Commands.EditCategory;
using Store.Application.Services.Products.Commands.EditProduct;
using Store.Application.Services.Products.Queries.GetCategories;
using Store.Application.Services.Products.Queries.GetCategoriesForNewProduct;
using Store.Application.Services.Products.Queries.GetProductDetailForAdmin;
using Store.Application.Services.Products.Queries.GetProductForAdmin;
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
        IAddNewProductService AddNewProductService { get; }
        IGetCategoreisForNewProductService GetCategoreisForNewProductService { get; }
        //<summary>
        //دریافت لیست محصولات برای پنل ادمین
        //</summary>
        IGetProductForAdminService GetProductForAdminService { get; }
        IGetProductDetailForAdminService GetProductDetailForAdminService { get; }
        IDeleteProductService DeleteProductService { get; }
        IEditProductService EditProductService { get; }
    }
}
