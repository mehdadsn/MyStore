using Microsoft.AspNetCore.Hosting;
using Store.Application.Interface.Context;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewCategory;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Commands.DeleteCategory;
using Store.Application.Services.Products.Commands.DeleteProduct;
using Store.Application.Services.Products.Commands.EditCategory;
using Store.Application.Services.Products.Commands.EditProduct;
using Store.Application.Services.Products.Queries.GetCategories;
using Store.Application.Services.Products.Queries.GetCategoriesForNewProduct;
using Store.Application.Services.Products.Queries.GetProductDetailForAdmin;
using Store.Application.Services.Products.Queries.GetProductDetailForSite;
using Store.Application.Services.Products.Queries.GetProductForAdmin;
using Store.Application.Services.Products.Queries.GetProductsForSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.FacadPattern
{
    public  class ProductFacad : IProductFacad
    {
        private readonly IDataBaseContext _context;
        private readonly IHostingEnvironment _environment;
        public ProductFacad(IDataBaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private AddNewCategoryService _addNewCategoryService;

        public AddNewCategoryService AddNewCategoryService
        {
            get
            {
                return _addNewCategoryService = _addNewCategoryService ?? new AddNewCategoryService(_context);
            }
        }

        private IGetCategoriesService _getCategoresService;
        public IGetCategoriesService GetCategoriesService
        {
            get{ 
                return _getCategoresService = _getCategoresService ?? new GetCategoriesService(_context);
            }
        }

        private IDeleteCategoryService _deleteCategoryService;
        public IDeleteCategoryService DeleteCategoryService
        {
            get
            {
                return _deleteCategoryService = _deleteCategoryService ?? new DeleteCategoryService (_context);
            }
        }

        private IEditCategoryService _editCategoryService;
        public IEditCategoryService EditCategoryService
        {
            get
            {
                return _editCategoryService = _editCategoryService ?? new EditCategoryService (_context);
            }
        }

        private IAddNewProductService _addNewProductService;
        public IAddNewProductService AddNewProductService
        {
            get
            {
                return _addNewProductService = _addNewProductService ?? new AddNewProductService (_context, _environment);
            }
        }

        private IGetCategoreisForNewProductService _getCategoreisForNewProductService;
        public IGetCategoreisForNewProductService GetCategoreisForNewProductService
        {
            get
            {
                return _getCategoreisForNewProductService = _getCategoreisForNewProductService ?? new GetCategoreisForNewProductService (_context);
            }
        }

        private IGetProductForAdminService _getProductForAdminService;
        public IGetProductForAdminService GetProductForAdminService
        {
            get
            {
                return _getProductForAdminService = _getProductForAdminService ?? new GetProductForAdminService (_context);
            }
        }

        private IGetProductDetailForAdminService _getProductDetailForAdminService;
        public IGetProductDetailForAdminService GetProductDetailForAdminService
        {
            get
            {
                return _getProductDetailForAdminService = _getProductDetailForAdminService ?? new GetProductDetailForAdmin(_context);
            }
        }

        private IDeleteProductService _deleteProductService;
        public IDeleteProductService DeleteProductService
        {
            get
            {
                return _deleteProductService = _deleteProductService ?? new DeleteProductService (_context);
            }
        }

        private IEditProductService _editProductService;
        public IEditProductService EditProductService
        {
            get
            {
                return _editProductService = _editProductService ?? new EditProductService (_context, _environment);
            }
        }

        private IGetProductsForSite _getProductsForSite;
        public IGetProductsForSite GetProductsForSite
        {
            get
            {
                return _getProductsForSite = _getProductsForSite ?? new GetProductsForSite (_context);
            }
        }

        private IGetProductDetailForSite _getProductDetailForSite;
        public IGetProductDetailForSite GetProductDetailForSite
        {
            get
            {
                return _getProductDetailForSite = _getProductDetailForSite ?? new GetProductDetailForSite (_context);
            }
        }

    }
}
