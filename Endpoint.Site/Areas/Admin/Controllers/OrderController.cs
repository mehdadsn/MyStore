using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Orders.Queries.GetUserOrders;
using Store.Common.Role;
using Store.Domain.Entities.Orders;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{ConstRoles.Admin},{ ConstRoles.Operator}")]

    public class OrderController : Controller
    {
        private readonly IGetOrdersForAdminService _getOrdersForAdminService;
        public OrderController(IGetOrdersForAdminService getOrdersForAdminService)
        {
            _getOrdersForAdminService = getOrdersForAdminService;
        }
        public IActionResult Index(OrderState orderState)
        {
            return View(_getOrdersForAdminService.Execute(orderState).Data);
        }
    }
}
