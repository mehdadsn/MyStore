using Endpoint.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Orders.Queries.GetUserOrders;
using Store.Common.Role;
using System.Security.Claims;

namespace Endpoint.Site.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IGetUserOrdersService _getUserOrdersService;
        public OrderController(IGetUserOrdersService getUserOrdersService)
        {
            _getUserOrdersService = getUserOrdersService;
        }
        public IActionResult Index()
        {
            long userId = ClaimUtility.GetUserId(User).Value;
            return View(_getUserOrdersService.Execute(userId).Data);
        }
    }
}
