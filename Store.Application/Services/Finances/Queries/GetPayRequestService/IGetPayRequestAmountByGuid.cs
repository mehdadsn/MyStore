using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Queries.GetPayRequestService
{
    public interface IGetPayRequestAmountByGuid
    {
        ResultDto<PayRequestAmountByGuidDto> Execute(Guid guid);
    }

    public class GetPayRequestAmountByGuid : IGetPayRequestAmountByGuid
    {
        private readonly IDataBaseContext _context;
        public GetPayRequestAmountByGuid(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<PayRequestAmountByGuidDto> Execute(Guid guid)
        {
            var payRequest = _context.PayRequests.Where(p => p.Guid == guid).FirstOrDefault();

            if(payRequest != null)
            {
                return new ResultDto<PayRequestAmountByGuidDto>()
                {
                    Data = new PayRequestAmountByGuidDto()
                    {
                        Amount = Convert.ToInt32(payRequest.Amount),
                        PayRequestId = payRequest.Id
                    },
                    IsSuccess = true

                };
            }
            else
            {
                throw new Exception("Pay Request Not Found");
            }
            
        }
    }

    public class PayRequestAmountByGuidDto
    {
        public long PayRequestId { get; set; }
        public int Amount { get; set; }
    }
}
