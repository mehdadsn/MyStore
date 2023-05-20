using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Queries.GetUserOrders
{
    public interface IGetUserOrdersService
    {
        ResultDto<List<GetUserOrdersDto>> Execute(long userId);
    }

    public class GetUserOrdersService : IGetUserOrdersService
    {
        private readonly IDataBaseContext _context;
        public GetUserOrdersService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<List<GetUserOrdersDto>> Execute(long userId)
        {
            var orders = _context.Orders
                .Where(p => p.UserId == userId)
                .Include(p => p.OrderDetails)
                .ThenInclude(p => p.Product)
                .OrderByDescending(p => p.Id)
                .ToList().Select(p => new GetUserOrdersDto()
                {
                    OrderId = p.Id,
                    OrderState = p.OrderState,
                    PayRequestId = p.PayRequestId,
                    OrderDetails = p.OrderDetails.Select(q => new OrderDetailsDto
                    {
                        OrderDetailId = q.Id,
                        count = q.Count,
                        price = Convert.ToInt32(q.Price),
                        ProductId = q.ProductId,
                        ProductName = q.Product.Name,
                    }).ToList(),

                }).ToList();

            return new ResultDto<List<GetUserOrdersDto>>()
            {
                Data = orders,
                IsSuccess = true
            };
        }
    }

    public class GetUserOrdersDto
    {
        public long OrderId { get; set; }
        public OrderState OrderState { get; set; }
        public long PayRequestId { get; set; }
        public List<OrderDetailsDto> OrderDetails { get; set; }
    }

    public class OrderDetailsDto
    {
        public long OrderDetailId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int price { get; set; }
        public int count { get; set; }
    }
}
