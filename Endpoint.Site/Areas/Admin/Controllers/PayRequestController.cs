using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Finances.Queries.GetPayRequestForAdmin;
using Store.Common.Role;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{ConstRoles.Admin},{ConstRoles.Operator}")]

    public class PayRequestController : Controller
    {
        private readonly IGetPayRequestForAdminService _getPayRequestForAdminService;
        public PayRequestController(IGetPayRequestForAdminService getPayRequestForAdminService)
        {
            _getPayRequestForAdminService = getPayRequestForAdminService;
        }
        public IActionResult Index()
        {
            return View(_getPayRequestForAdminService.Execute().Data);
        }
    }
}
