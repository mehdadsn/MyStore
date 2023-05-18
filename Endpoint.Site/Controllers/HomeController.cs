using Endpoint.Site.Models;
using Endpoint.Site.Models.ViewModels.HomePage;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Common.Queries.GetHomepageimages;
using Store.Application.Services.Common.Queries.GetSlider;
using System.Diagnostics;
using Store.Application.Services.Products.Queries.GetProductsForSite;

namespace Endpoint.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetSliderService _getSliderService;
        private readonly IGetHomepageImagesService _getHomepageImagesService;
        private readonly IProductFacad _productFacad;

        public HomeController(ILogger<HomeController> logger,
            IGetSliderService getSliderService,
            IGetHomepageImagesService getHomepageImagesService,
            IProductFacad productFacad)
        {
            _logger = logger;
            _getSliderService = getSliderService;
            _getHomepageImagesService = getHomepageImagesService;
            _productFacad = productFacad;
        }

        public IActionResult Index()
        {
            HomePageViewModel homepage = new HomePageViewModel()
            {
                Sliders = _getSliderService.Execute().Data,
                Images = _getHomepageImagesService.Execute().Data,
                Laptop = _productFacad.GetProductsForSite.Execute(Ordering.Newest,null,1,6,1).Data.Products,
            };

            return View(homepage);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}