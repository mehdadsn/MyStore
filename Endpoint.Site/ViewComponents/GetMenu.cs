using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Common.Queries.GetMenuItems;

namespace Endpoint.Site.ViewComponents
{
    public class GetMenu : ViewComponent
    {
        private readonly IGetMenuItemsServices _getMenuItemsServices;
        public GetMenu(IGetMenuItemsServices getMenuItemsServices)
        {
            _getMenuItemsServices = getMenuItemsServices;
        }

        public IViewComponentResult Invoke()
        {
            var menuItems = _getMenuItemsServices.Execute();
            return View(viewName:"GetMenu",menuItems.Data);
        }
    }
}
