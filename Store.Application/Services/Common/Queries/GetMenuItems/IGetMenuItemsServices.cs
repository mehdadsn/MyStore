using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Common.Queries.GetMenuItems
{
    public interface IGetMenuItemsServices
    {
        ResultDto<List<MenuItemsDto>> Execute();
    }

    public class GetMenuItemsServices: IGetMenuItemsServices
    {
        private readonly IDataBaseContext _context;
        public GetMenuItemsServices(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<MenuItemsDto>> Execute()
        {
            var categoris = _context.Categories
                .Include(p => p.ChildCategories)
                .Where(p => p.ParentCategoryId == null)
                .ToList()
                .Select(p => new MenuItemsDto()
            {
                CategoryId = p.Id,
                Name = p.Name,
                Child = p.ChildCategories.ToList().Select(child => new MenuItemsDto()
                {
                    Name = child.Name,
                    CategoryId = child.Id,
                }).ToList(),
            }).ToList();

            return new ResultDto<List<MenuItemsDto>>() {
                Data = categoris,
                IsSuccess = true
            };
        }
    }

    public class MenuItemsDto
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public List<MenuItemsDto> Child { get; set; }
    }
}
