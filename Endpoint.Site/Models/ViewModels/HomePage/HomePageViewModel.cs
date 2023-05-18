using Store.Application.Services.Common.Queries.GetHomepageimages;
using Store.Application.Services.Common.Queries.GetSlider;
using Store.Application.Services.Products.Queries.GetProductsForSite;

namespace Endpoint.Site.Models.ViewModels.HomePage
{
    public class HomePageViewModel
    {
        public List<SliderDto> Sliders { get; set; }
        public List<HomepageImagesDto> Images { get; set; }
        public List<ProductForSiteDto> Laptop { get; set; }

    }
}
