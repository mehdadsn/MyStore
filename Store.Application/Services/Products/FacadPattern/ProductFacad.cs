using Store.Application.Interface.Context;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewCategory;
using Store.Application.Services.Products.Commands.DeleteCategory;
using Store.Application.Services.Products.Commands.EditCategory;
using Store.Application.Services.Products.Queries.GetCategories;
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
        public ProductFacad(IDataBaseContext context)
        {
            _context = context;
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
        
    }
}
