using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Queries.GetPayRequestForAdmin
{
    public interface IGetPayRequestForAdminService
    {
        ResultDto<List<PayRequestDto>> Execute();
    }

    public class GetPayRequestForAdminService : IGetPayRequestForAdminService
    {
        private readonly IDataBaseContext _context;
        public GetPayRequestForAdminService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<PayRequestDto>> Execute()
        {
            var payRequests = _context.PayRequests.Include(p=>p.User).Select(p => new PayRequestDto
            {
                Id = p.Id,
                Amount = p.Amount,
                Authority = p.Authority,
                Completed = p.Completed,
                Guid = p.Guid,
                PayTime = p.PayTime,
                UserId = p.UserId,
                RefId = p.RefId,
                UserName = p.User.FullName
            }).ToList();

            return new ResultDto<List<PayRequestDto>>()
            {
                Data = payRequests,
                IsSuccess = true
            };
        }
    }

    public class PayRequestDto
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string  UserName { get; set; }
        public long UserId { get; set; }
        public float Amount { get; set; }
        public bool Completed { get; set; }
        public DateTime? PayTime { get; set; }
        public string? Authority { get; set; }
        public long RefId { get; set; } = 0;
        public virtual List<Order> Orders { get; set; }
    }
}
