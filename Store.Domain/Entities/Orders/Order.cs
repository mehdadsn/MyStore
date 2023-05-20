using Store.Domain.Entities.Commons;
using Store.Domain.Entities.Finances;
using Store.Domain.Entities.Products;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public virtual User User { get; set; }
        public long UserId { get; set; } 
        public virtual PayRequest PayRequest { get; set; }
        public long PayRequestId { get; set; }
        public OrderState OrderState { get; set; }
        public string Address { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }

    public class OrderDetail : BaseEntity
    {
        public virtual Order Order { get; set; }
        public long OrderId { get; set; }
        public virtual Product Product { get; set; }
        public long ProductId { get; set; }
        public float Price { get; set; }
        public int Count { get; set; }
    }

    public enum OrderState
    {
        [Display(Name ="در حال پردازش")]
        Processing = 0,
        [Display(Name = "لغو شده")]
        Canceled = 1,
        [Display(Name = "تحویل شده")]
        Delivered = 2,
    }
}
