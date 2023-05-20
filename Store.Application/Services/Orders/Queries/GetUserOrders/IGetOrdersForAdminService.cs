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
    public interface IGetOrdersForAdminService
    {
        ResultDto<List<OrdersDto>> Execute(OrderState orderState);
    }

    public class GetOrdersForAdminService : IGetOrdersForAdminService
    {
        private readonly IDataBaseContext _context;
        public GetOrdersForAdminService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<OrdersDto>> Execute(OrderState orderState)
        {
            var orders = _context.Orders.Include(p => p.OrderDetails)
                .Where(p => p.OrderState == orderState)
                .OrderByDescending(p=>p.Id).ToList().Select(p=> new OrdersDto()
                {
                    InsertTime = p.InsertTime,
                    orderId = p.Id,
                    OrderState = p.OrderState,
                    ProductCount = p.OrderDetails.Count(),
                    payRequestId = p.PayRequestId,
                    UserId = p.UserId,

                }).ToList();

            return new ResultDto<List<OrdersDto>>()
            {
                Data = orders,
                IsSuccess = true
            };
        }
    }

    public class OrdersDto
    {
        public long orderId { get; set; }
        public DateTime InsertTime { get; set; }
        public long payRequestId { get; set; }
        public long UserId { get; set; }
        public OrderState OrderState { get; set; }
        public int ProductCount { get; set; }
    }
}
