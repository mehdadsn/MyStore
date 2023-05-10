using Microsoft.AspNetCore.Mvc;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Products.Commands.EditCategory;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IProductFacad _productFacad;
        public CategoriesController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }
        public IActionResult Index(long? parentId)
        {
            return View(_productFacad.GetCategoriesService.Execute(parentId).Data);
        }

        [HttpGet]
        public IActionResult AddNewCategory(long? parentId)
        {
            ViewBag.ParentId = parentId;
            return View();
        }

        [HttpPost]
        public IActionResult AddNewCategory(long? parentId, string Name)
        {
            var result = _productFacad.AddNewCategoryService.Execute(parentId, Name);
            return Json(result);
        }

        [HttpPost]
        public IActionResult Delete(long categoryId)
        {
            return Json(_productFacad.DeleteCategoryService.Execute(categoryId));
        }

        [HttpPost]
        public IActionResult Edit(long categoryId, string categoryName) {
            return Json(_productFacad.EditCategoryService.Execute(new RequestEditCategoryDto
            {
                CategoryId = categoryId,
                Name = categoryName
            }));
        }
    }
}
