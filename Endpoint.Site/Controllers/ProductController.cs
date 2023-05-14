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
        public IActionResult Index(int page = 1)
        {
            return View(_productFacad.GetProductsForSite.Execute(page).Data);
        }

        public IActionResult Detail(long id) {
            return View(_productFacad.GetProductDetailForSite.Execute(id).Data);
        }
    }
}
