using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.HomePage.AddNewSliderService;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly IAddNewSliderService _addNewSliderService;
        public SliderController(IAddNewSliderService addNewSliderService)
        {
            _addNewSliderService = addNewSliderService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(IFormFile file, string link)
        {
            _addNewSliderService.Execute(file, link);
            return View();
        }
    }
}
