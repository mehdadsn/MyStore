using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewProduct;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IProductFacad _prodoctFacad;
        public ProductController(IProductFacad productFacad)
        {
            _prodoctFacad = productFacad;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            ViewBag.Categories = new SelectList(_prodoctFacad.GetCategoreisForNewProductService.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(RequestAddNewProductDto request, List<AddNewProductFeatures> features)
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                images.Add(file);
            }

            request.Images = images;
            request.Features = features;
            return Json(_prodoctFacad.AddNewProductService.Execute(request));
        }
    }
}
