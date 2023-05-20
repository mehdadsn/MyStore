using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Finances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Commands.AddPyaRequest
{
    public interface IAddPayRequestService
    {
        ResultDto<ResultPayRequestDto> Execute(float amount, long userId);
    }

    public class AddPayRequestService : IAddPayRequestService
    {
        private readonly IDataBaseContext _context;
        public AddPayRequestService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultPayRequestDto> Execute(float amount, long userId)
        {
            var user = _context.Users.Find(userId);
            PayRequest payRequest = new PayRequest()
            {
                Amount = amount,
                Guid = Guid.NewGuid(),
                Completed = false,
                User = user,
                InsertTime = DateTime.Now,
                UserId = userId,
                
            };
            _context.PayRequests.Add(payRequest);
            _context.SaveChanges();

            return new ResultDto<ResultPayRequestDto>()
            {
                Data = new ResultPayRequestDto()
                {
                    guid = payRequest.Guid,
                    Amount = payRequest.Amount,
                    Email = user.Email,
                    PayRequestId = payRequest.Id
                },
                IsSuccess = true
            };
        }
    }

    public class ResultPayRequestDto
    {
        public Guid guid { get; set; }
        public float Amount { get; set; }
        public string Email { get; set; }
        public long PayRequestId { get; set; }
    }
}
