using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Carts
{
    public interface ICartService
    {
        ResultDto AddToCart(long productId, Guid browserId);
        ResultDto RemoveFromCart(long cartItemId, Guid browserId);
        ResultDto<CartDto> GetCart(Guid browserId, long? userId);
        ResultDto IncreaseCount(long cartItemId);
        ResultDto DecreaseCount(long cartItemId);


    }

    public class CartService : ICartService
    {
        private readonly IDataBaseContext _context;
        public CartService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto AddToCart(long productId, Guid browserId)
        {
            var cart = _context.Carts.Where(p => p.BrowserId == browserId && p.Finished==false).FirstOrDefault();
            if(cart == null)
            {
                cart = new Cart()
                {
                    Finished = false,
                    BrowserId = browserId,
                    InsertTime = DateTime.Now,
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var product = _context.Products.Find(productId);

            var cartItem = _context.CartItems.Where(p => p.ProductId == productId && p.CartId == cart.Id).FirstOrDefault();
            if(cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                cartItem = new CartItem()
                {
                    Cart = cart,
                    Count = 1,
                    Price = product.Price,
                    Product = product,
                    InsertTime= DateTime.Now,
                };
                _context.CartItems.Add(cartItem);
            }
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = $"محصول {product.Name} با موفقیت به سبد شما اضافه شد."
            };

        }

        public ResultDto DecreaseCount(long cartItemId)
        {
            var cartItem = _context.CartItems.Find(cartItemId);
            if(cartItem.Count == 1)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Count--;
            }
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
            };
        }

        public ResultDto<CartDto> GetCart(Guid browserId, long? userId)
        {
            var cart = _context.Carts
                .Include(p => p.CartItems)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(p => p.BrowserId == browserId && p.Finished==false)
                .OrderByDescending(p=>p.Id)
                .FirstOrDefault();
            if(cart == null)
            {
                return new ResultDto<CartDto>()
                {
                    Data = new CartDto()
                    {
                        CartItems = new List<CartItemDto>(),
                        ProductCount = 0,
                        TotalPrice = 0,
                    }
                };
            }
            if(userId != null)
            {
                var user = _context.Users.Find(userId);
                cart.User = user;
                _context.SaveChanges();
            }

            return new ResultDto<CartDto>()
            {
                Data = new CartDto()
                {
                    CartId = cart.Id,
                    ProductCount = cart.CartItems.Count,
                    TotalPrice = cart.CartItems.Sum(p => p.Price * p.Count),
                    CartItems = cart.CartItems.Select(p => new CartItemDto()
                    {
                        Id = p.Id,
                        Count = p.Count,
                        Price = p.Price,
                        Image = p.Product?.ProductImages?.FirstOrDefault()?.Src??"",
                        ProductName = p.Product.Name,
                        ProductId = p.ProductId,
                    }).ToList(),
                },
                IsSuccess = true
            };
        }

        public ResultDto IncreaseCount(long cartItemId)
        {
            var cartItem = _context.CartItems.Find(cartItemId);
            cartItem.Count++;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
            };
        }

        public ResultDto RemoveFromCart(long cartItemId, Guid browserId)
        {
            var cartItem = _context.CartItems.Where(p => p.Cart.BrowserId == browserId && p.Id == cartItemId).FirstOrDefault();
            if(cartItem != null)
            {
                cartItem.IsRemoved = true;
                cartItem.RemoveTime = DateTime.Now;
                _context.SaveChanges();
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "محصول از سبد حذف شد."
                };
            }
            else
            {
                return new ResultDto()
                {
                    IsSuccess =false,
                    Message = "محصول یافت نشد."
                };
            }
        }
    }


    public class CartDto
    {
        public long CartId { get; set; }
        public int ProductCount { get; set; }
        public float TotalPrice { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }

    public class CartItemDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        public float Price { get; set; } 
    }
}
