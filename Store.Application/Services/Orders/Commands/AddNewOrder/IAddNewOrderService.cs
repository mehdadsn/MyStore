using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Commands.AddNewOrder
{
    public interface IAddNewOrderService
    {
        ResultDto Execute(RequestAddNewOrderServiceDto request);
    }
    public class AddNewOrderService : IAddNewOrderService
    {
        private readonly IDataBaseContext _context;
        public AddNewOrderService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestAddNewOrderServiceDto request)
        {
            var user = _context.Users.Find(request.UserId);
            var payRequest = _context.PayRequests.Find(request.UserId);
            var cart = _context.Carts.Include(p => p.CartItems).ThenInclude(p => p.Product).Where(p => p.Id == request.CartId).FirstOrDefault();

            payRequest.Completed = true;
            payRequest.PayTime = DateTime.Now;
            //payRequest.RefId = request.RefId;
            payRequest.Authority = payRequest.Authority;
            cart.Finished = true;

            Order order = new Order()
            {
                Address = "",
                OrderState = OrderState.Processing,
                PayRequest = payRequest,
                User = user,
                UserId = request.UserId,
                InsertTime = DateTime.Now,
                
            };

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach(var item in cart.CartItems)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    Count = item.Count,
                    Order = order,
                    Price = item.Product.Price,
                    Product = item.Product,

                };
                orderDetails.Add(orderDetail);
            }
            order.OrderDetails = orderDetails;
            _context.Orders.Add(order);
            _context.OrderDetails.AddRange(orderDetails);
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
            };
        }
    }

    public class RequestAddNewOrderServiceDto
    {
        public long CartId { get; set; }
        public long PayRequestId { get; set; }
        public long UserId { get; set; }
        public long RefId { get; set; } = 0;
        public string Authority { get; set; }

    }
}
