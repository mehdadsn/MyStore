using Dto.Payment;
using Endpoint.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Carts;
using Store.Application.Services.Finances.Commands.AddPyaRequest;
using Store.Application.Services.Finances.Queries.GetPayRequestService;
using Store.Application.Services.Orders.Commands.AddNewOrder;
using Store.Common.Role;
using ZarinPal.Class;

namespace Endpoint.Site.Controllers
{
    [Authorize(ConstRoles.Customer)]
    public class PayController : Controller
    {
        private readonly IAddPayRequestService _addPayRequestService;
        private readonly ICartService _cartService;
        private readonly CookieManger _cookieManger;
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;
        private readonly IGetPayRequestAmountByGuid _payRequestAmountByGuid;
        private readonly IAddNewOrderService _addNewOrderService;

        public PayController(IAddPayRequestService addPayRequestService, ICartService cartService, IGetPayRequestAmountByGuid payRequestAmountByGuid, IAddNewOrderService addNewOrderService)
        {
            _addPayRequestService = addPayRequestService;
            _cartService = cartService;
            _cookieManger = new CookieManger();
            _payRequestAmountByGuid = payRequestAmountByGuid;
            _addNewOrderService = addNewOrderService;

            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
        }
        public async Task<IActionResult> Index()
        {
            long? userId = ClaimUtility.GetUserId(User);
            var cart = _cartService.GetCart(_cookieManger.GetBrowserId(HttpContext), userId).Data;
            if(cart.TotalPrice > 0)
            {
                var payRequest = _addPayRequestService.Execute(cart.TotalPrice, userId.Value).Data;
                
                // ارسال به درگاه

                var result = await _payment.Request(new DtoRequest()
                {
                    Mobile = "09121112222",
                    CallbackUrl = $"https://localhost:44316/pay/validate?guid={payRequest.guid}",
                    Description = "پرداخت فاکتور شماره " + payRequest.PayRequestId,
                    Email = payRequest.Email,
                    Amount = Convert.ToInt32(payRequest.Amount),
                    MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
                }, ZarinPal.Class.Payment.Mode.sandbox);
                return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{result.Authority}");

            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }

            

        }

        public async Task<IActionResult> Validate(Guid guid, string authority, string status)
        {
            var payRequest = _payRequestAmountByGuid.Execute(guid).Data;
            var verification = await _payment.Verification(new DtoVerification
            {
                Amount = payRequest.Amount,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                Authority = authority
            }, Payment.Mode.sandbox);

            long? userId = ClaimUtility.GetUserId(User);
            var cart = _cartService.GetCart(_cookieManger.GetBrowserId(HttpContext), userId).Data;

            if (verification.Status == 100)
            {
                _addNewOrderService.Execute(new RequestAddNewOrderServiceDto()
                {
                    CartId = cart.CartId,
                    UserId = userId.Value,
                    PayRequestId = payRequest.PayRequestId,
                    Authority = authority,
                });
                return RedirectToAction("Index","Order");
            }
            else
            {
                return View();

            }
        }
    }
}
