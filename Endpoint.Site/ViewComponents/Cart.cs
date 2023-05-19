using Endpoint.Site.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Carts;

namespace Endpoint.Site.ViewComponents
{
    public class Cart : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly CookieManger _cookieManager;
        public Cart(ICartService cartService)
        {
            _cartService = cartService;
            _cookieManager = new CookieManger();
        }
        
        public IViewComponentResult Invoke()
        {
            var userId = ClaimUtility.GetUserId(HttpContext.User);

            return View(viewName: "Cart", _cartService.GetCart(_cookieManager.GetBrowserId(HttpContext), userId).Data);
        }
    }
}
