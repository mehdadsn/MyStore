using Microsoft.AspNetCore.Mvc;
using Store.Application.Interface.FacadPatterns;

namespace Endpoint.Site.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductFacad _productFacad;
        public ProductController(IProductFacad productfacad)
        {
            _productFacad = productfacad;
        }
        public IActionResult Index(string searchKey,int page = 1, long? categoryId = null)
        {
            return View(_productFacad.GetProductsForSite.Execute(searchKey,page, categoryId).Data);
        }

        public IActionResult Detail(long id) {
            return View(_productFacad.GetProductDetailForSite.Execute(id).Data);
        }
    }
}
