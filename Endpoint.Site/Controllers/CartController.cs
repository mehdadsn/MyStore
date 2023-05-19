using Endpoint.Site.Utilities;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Carts;

namespace Endpoint.Site.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly CookieManger cookieManger;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            cookieManger = new CookieManger();

        }
        public IActionResult Index()
        {
            var userId = ClaimUtility.GetUserId(User);
            var getCartResult = _cartService.GetCart(cookieManger.GetBrowserId(HttpContext),userId);
            return View(getCartResult.Data);
        }

        public IActionResult AddToCart(long productId)
        {

            var resultAdd = _cartService.AddToCart(productId, cookieManger.GetBrowserId(HttpContext));
            return RedirectToAction("Index");
        }

        public IActionResult IncreaseCount(long cartItemId)
        {
            _cartService.IncreaseCount(cartItemId);
            return RedirectToAction("Index");
        }
        public IActionResult DecreaseCount(long cartItemId)
        {
            _cartService.DecreaseCount(cartItemId);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(long cartItemId) 
        {
            _cartService.RemoveFromCart(cartItemId, cookieManger.GetBrowserId(HttpContext));
            return RedirectToAction("Index");
        }
    }
}
