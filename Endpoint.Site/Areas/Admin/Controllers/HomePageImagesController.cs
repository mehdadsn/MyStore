using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.HomePage.AddHomepageImages;
using Store.Common.Role;
using Store.Domain.Entities.HomePage;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstRoles.Admin)]

    public class HomePageImagesController : Controller
    {
        private readonly IAddHomepageImagesService _addHomepageImagesService;
        public HomePageImagesController(IAddHomepageImagesService addHomepageImagesService)
        {
            _addHomepageImagesService = addHomepageImagesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(IFormFile file, string link, ImageLocation imageLocation) 
        {
            _addHomepageImagesService.Execute(new RequestAddHomepageImagesDto()
            {
                File = file,
                ImageLocation = imageLocation,
                Link = link
            });
            return View();
        }
    }
}
