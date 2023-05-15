using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Common.Queries.GetCategoryForSearchBar;

namespace Endpoint.Site.ViewComponents
{
    public class Search : ViewComponent
    {
        private readonly IGetCategoryServiceForSearchBar _getCategoryServiceForSearchBar;
        public Search(IGetCategoryServiceForSearchBar getCategoryServiceForSearchBar)
        {
            _getCategoryServiceForSearchBar = getCategoryServiceForSearchBar;
        }

        public IViewComponentResult Invoke()
        {
            return View(viewName:"Search", _getCategoryServiceForSearchBar.Execute().Data);
        }
    }
}
