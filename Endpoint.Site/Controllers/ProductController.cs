using Microsoft.AspNetCore.Mvc;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Products.Queries.GetProductsForSite;

namespace Endpoint.Site.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductFacad _productFacad;
        public ProductController(IProductFacad productfacad)
        {
            _productFacad = productfacad;
        }
        public IActionResult Index(Ordering ordering, string searchKey,int page = 1,int pageSize = 20, long? categoryId = null)
        {
            return View(_productFacad.GetProductsForSite.Execute(ordering, searchKey, page, pageSize, categoryId).Data);
        }

        public IActionResult Detail(long id) {
            return View(_productFacad.GetProductDetailForSite.Execute(id).Data);
        }
    }
}
