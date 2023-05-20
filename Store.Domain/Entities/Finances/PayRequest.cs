using Store.Domain.Entities.Commons;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Finances
{
    public class PayRequest : BaseEntity
    {
        public Guid Guid { get; set; }
        public virtual User User { get; set; }
        public long UserId { get; set; }
        public float Amount { get; set; }
        public bool Completed { get; set; }
        public DateTime? PayTime { get; set; }
        public string? Authority { get; set; }
        public long RefId { get; set; } = 0;
        public virtual List<Order> Orders{ get; set; }

    }
}
